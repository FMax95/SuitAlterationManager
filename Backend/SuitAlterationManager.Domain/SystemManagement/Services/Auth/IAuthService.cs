using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Services.Auth
{
	public interface IAuthService
	{
		Task<AuthResult> Authenticate(string email, string password, string ipAddress);
		Task<AuthResult> RefreshToken(string token, string ipAddress);
		Task<bool> RevokeToken(string token, string ipAddress);
		ClaimsIdentity CreateIdentity(User user, IEnumerable<Group> groups);
		Task ForgotPassword(string email, string clientUrl);
		Task<bool> ValidateResetToken(string token);
		Task ResetPassword(string token, string newPassword);
		string GenerateJwtToken(User user, IEnumerable<Group> groups);
        JwtSecurityToken ValidateJwtToken(string token);
    }
}
