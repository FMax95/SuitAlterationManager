using CSharpFunctionalExtensions;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SuitAlterationManager.Extensions
{
	[Controller]
	public abstract class BaseController : ControllerBase
	{
		public UserID LoggedUserId => (UserID)HttpContext.Items[nameof(LoggedUserId)];
		protected IActionResult NOk() => base.Ok(Envelope.Success(false));
		protected new IActionResult Ok() => base.Ok(Envelope.Success(true));
		protected IActionResult Ok<T>(T result) => base.Ok(Envelope.Success(result));
		protected IActionResult Created<T>(T result) => base.Created(string.Empty, Envelope.Success(result));
		protected new IActionResult BadRequest(object error) => base.BadRequest(Envelope.Failure(error));
		protected new IActionResult Unauthorized(object error) => base.Unauthorized(Envelope.Failure(error));
		protected new IActionResult NotFound(object error) => base.NotFound(Envelope.Failure(error));
		protected IActionResult FromResult(Result result)
		{
			var (isSuccess, _, error) = result;
			return isSuccess ? Ok() : BadRequest(error);
		}

		protected void SetTokenCookie(string token)
		{
			var cookieOptions = new CookieOptions
			{
        // TODO: uncomment only if the api project and cms project will be published under different domain. If is not the case, you can delete them
        // NOTE: if you need to uncomment them, remember that this work only on https (you can start the project in localhost with https in either mac or windows, or increment token duration from appsettings.json)
        // SameSite = SameSiteMode.None,
        // Secure = true,
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddDays(7)
			};

			Response.Cookies.Append("refreshToken", token, cookieOptions);
		}

		protected string GetIpAddress()
		{
			if (Request.Headers.ContainsKey("X-Forwarded-For"))
				return Request.Headers["X-Forwarded-For"];
			return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
		}
	}
}