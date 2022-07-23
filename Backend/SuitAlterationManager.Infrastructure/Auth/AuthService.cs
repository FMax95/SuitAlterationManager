using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.Services;
using SuitAlterationManager.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SuitAlterationManager.Domain.SystemManagement.Services.Auth;
using BC = BCrypt.Net.BCrypt;
using SuitAlterationManager.Infrastructure.Email.Messages;
using SuitAlterationManager.Infrastructure.Email;

namespace SuitAlterationManager.Infrastructure.Auth
{
    public class AuthService : IAuthService
	{
		private readonly IUserRepository users;
		private readonly IEmailService emailService;
		private readonly IGroupRepository groups;
		private readonly AuthOptions authOptions;
		private readonly DbContext context;

		public AuthService(IOptions<AuthOptions> authOptions, IUserRepository users, IGroupRepository groups, DbContext context, IEmailService emailService)
		{
			this.authOptions = authOptions.Value;
			this.users = users;
			this.groups = groups;
			this.context = context;
			this.emailService = emailService;
		}

		public async Task<AuthResult> Authenticate(string email, string password, string ipAddress)
		{
			return await context.Execute(async () =>
			{
				var user = await users.GetByEmailAsync(email);

				if (user is null || !BC.Verify(password, user.Password))
					return null;

				// authentication successful so generate jwt and refresh tokens
				var userGroups = await groups.GetManyAsync(user.Groups.Select(g => g.IdGroup));
				var jwtToken = GenerateJwtToken(user, userGroups);
				var refreshToken = user.GenerateToken(ipAddress, DateTimeOffset.UtcNow, authOptions.RefreshTokenDaysLifetime);
				user.RemoveOldTokens(authOptions.RefreshTokenDaysTTL);

				// save refresh token
				users.Update(user);

				return new AuthResult(user, jwtToken, refreshToken.Token, userGroups);
			});
		}

		public async Task<AuthResult> RefreshToken(string oldRefreshToken, string ipAddress)
		{
			return await context.Execute(async () =>
			{
				var user = await users.GetByRefreshTokenAsync(oldRefreshToken);

				if (user is null)
					return null;

				// replace old refresh token with a new one and save
				var newRefreshToken = user.RefreshAuthToken(oldRefreshToken, ipAddress, DateTimeOffset.UtcNow, authOptions.RefreshTokenDaysLifetime);
				user.RemoveOldTokens(authOptions.RefreshTokenDaysTTL);
				users.Update(user);

				// generate new jwt
				var userGroups = await groups.GetManyAsync(user.Groups.Select(g => g.IdGroup));
				var jwtToken = GenerateJwtToken(user, userGroups);

				return new AuthResult(user, jwtToken, newRefreshToken.Token);
			});
		}

		public async Task<bool> RevokeToken(string token, string ipAddress)
		{
			return await context.Execute(async () =>
			{
				var user = await users.GetByRefreshTokenAsync(token);

				if (user is null)
					return false;

				// revoke token and save
				var revoked = user.RevokeToken(token, ipAddress, DateTimeOffset.UtcNow);

				if (!revoked)
					return false;

				users.Update(user);

				return true;
			});
		}

		public ClaimsIdentity CreateIdentity(User user, IEnumerable<Group> groups)
		{
			var claims = new List<Claim>
			{
				new Claim("sub", user.Id.Value.ToString()),
				new Claim(ClaimTypes.GivenName, user.UserInformation?.FirstName),
				new Claim(ClaimTypes.Surname, user.UserInformation?.LastName),
				new Claim(ClaimTypes.Email, user.Email)
			};
			claims.AddRange(groups.Select(g => new Claim(ClaimTypes.Role, g.Name)));

			return new ClaimsIdentity(claims);
		}

		public async Task ForgotPassword(string email, string clientUrl)
		{
            await context.Execute(async () =>
            {
                var user = await users.GetByEmailAsync(email)
                    ?? throw new DomainException(ErrorCodes.UserDoesNotExist, $"Unable to find user with email: {email}");
                // generate reset token and update
                var resetToken = user.GenerateResetToken(DateTimeOffset.UtcNow, authOptions.ResetTokenMinutesLifetime);
                users.Update(user);

                var resetUrl = $"{clientUrl}?token={user.ResetToken}";

                var message = new RecoveryPassword(emailService)
                {
                    To = user.Email
                };
                await message.buildBody(
                    new Dictionary<string, string>() {
                        { "UserInfo", $"{user.UserInformation.FirstName} {user.UserInformation.LastName}" },
                        { "Link", resetUrl }
                    }
                );

                await emailService.SendAsync(message);
            });
        }

		public async Task<bool> ValidateResetToken(string token)
		{
			var user = await users.GetByResetTokenAsync(token);

			return !(user is null) && !(user.ResetTokenExpirationDate < DateTimeOffset.UtcNow);
		}

		public async Task ResetPassword(string token, string newPassword)
		{
			await context.Execute(async () =>
			{
				var user = await users.GetByResetTokenAsync(token);

				if (user is null || user.ResetTokenExpirationDate < DateTimeOffset.UtcNow)
					throw new DomainException(ErrorCodes.InvalidResetToken, "Invalid reset token!");

				user.ResetPassword(newPassword);
				users.Update(user);
			});
		}

		public string GenerateJwtToken(User user, IEnumerable<Group> groups)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(authOptions.Secret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = CreateIdentity(user, groups),
				Expires = DateTime.UtcNow.AddMinutes(authOptions.TokenMinutesLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public JwtSecurityToken ValidateJwtToken(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out var validatedToken);

				return (JwtSecurityToken)validatedToken;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
