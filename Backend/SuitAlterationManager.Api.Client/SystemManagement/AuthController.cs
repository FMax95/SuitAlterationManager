using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuitAlterationManager.Api.Client.SystemManagement.Models;
using SuitAlterationManager.Api.Client.SystemManagement.Responses;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement.Services.Auth;
using SuitAlterationManager.Extensions;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
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
                Email = result.Email,
                JwtToken = result.Token
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

            return Ok(response);
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

            return !response
                ? NotFound(new Error(ErrorCodes.TokenNotFound, "Token not found"))
                : Ok(new {message = "Token revoked"});
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