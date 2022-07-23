using SuitAlterationManager.Api.CMS.SystemManagement.Models;
using SuitAlterationManager.Api.CMS.SystemManagement.Responses;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement.Services;
using SuitAlterationManager.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SuitAlterationManager.Domain.SystemManagement.Services.Auth;
using SuitAlterationManager.Api.CMS.SystemManagement.Queries;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;

namespace SuitAlterationManager.Api.CMS.SystemManagement
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService authService;
        private readonly IPermissionQueries permissionQueries;

        public AuthController(IAuthService authService, IPermissionQueries permissionQueries)
        {
            this.authService = authService;
            this.permissionQueries = permissionQueries;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest model)
        {
            var result = await authService.Authenticate(model.Email, model.Password, GetIpAddress());
            if (result == null)
                return NOk();

            SetTokenCookie(result.RefreshToken);

            return Ok(new AuthResponse
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Image = result.Image,
                Email = result.Email,
                Token = result.Token,
                Permissions = await permissionQueries.GetUserPermissionsAsync(new UserID(result.Id))
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await authService.RefreshToken(refreshToken, GetIpAddress());

            if (response == null)
                return Unauthorized(new Error(ErrorCodes.InvalidToken, "Invalid token"));

            SetTokenCookie(response.RefreshToken);

            return Ok(new AuthResponse
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Image = response.Image,
                Email = response.Email,
                Token = response.Token,
                Permissions = await permissionQueries.GetUserPermissionsAsync(new UserID(response.Id))
            });
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new Error(ErrorCodes.TokenIsRequired, "Token is required"));

            var response = await authService.RevokeToken(token, GetIpAddress());

            return !response ?
                NotFound(new Error(ErrorCodes.TokenNotFound, "Token not found")) :
                Ok(new { message = "Token revoked" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            await authService.ForgotPassword(model.Email, $"{Request.Headers["origin"]}/reset-password");
            return Ok();
        }

        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenRequest model)
        {
            var isValid = await authService.ValidateResetToken(model.Token);

            if (!isValid)
                return BadRequest(new Error(ErrorCodes.InvalidResetToken, "Reset token is invalid or expired!"));

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            await authService.ResetPassword(model.Token, model.Password);
            return Ok();
        }
    }
}
