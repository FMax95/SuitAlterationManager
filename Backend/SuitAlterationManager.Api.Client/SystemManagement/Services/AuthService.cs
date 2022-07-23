using Microsoft.IdentityModel.Tokens;
using SuitAlterationManager.Api.Client.SystemManagement.Models;
using SuitAlterationManager.Api.Client.SystemManagement.Queries;
using SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.Base.Validation;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace SuitAlterationManager.Api.Client.SystemManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserQueries userQueries;
        public AuthService(IUserQueries userQueries)
        {
            this.userQueries = userQueries;
        }
        public async Task<AuthResponse> Authenticate(string email, string password)
        {
            var user = await this.userQueries.FindUserByEmailAsync(email);

            if (user == null)
                throw new ApplicationException(ApplicationServiceExceptionCode.WrongEmail);

            if (!VerifyPassword(password, user))
                throw new ApplicationException(ApplicationServiceExceptionCode.WrongPassword);

            return new AuthResponse()
            {
                Email = user.Email,
                Id = user.Id,
                Token = GenerateJwtToken(user.Id,user.Email)
            };
        }

        private string GenerateJwtToken(Guid id, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("C06F641A-5D6F-47CC-BF91-2F547542FD07");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = CreateIdentity(id,email),
                Expires = DateTime.UtcNow.AddMinutes(360),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity CreateIdentity(Guid id, string email)
        {
            var claims = new List<Claim>
            {
                new Claim("sub", id.ToString()),
                new Claim(ClaimTypes.Email, email)
            };
            
            return new ClaimsIdentity(claims);
        }

        private static bool VerifyPassword(string password, Responses.UserResponse user)
        {
            if (!BC.Verify(password, user.Password))
                return false;
            return true;
        }
    }
}
