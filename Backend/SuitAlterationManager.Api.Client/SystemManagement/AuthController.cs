using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Api.Client.SystemManagement.Models;
using Microsoft.Extensions.Logging;
using System;
using SuitAlterationManager.Api.Client.Base;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/auth")]
	public class AuthController : BaseController
	{
		private readonly IAuthService authService;

		public AuthController(
			IAuthService authService)
		{
			this.authService = authService;
		}

		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] AuthModel model)
		{
			var result = await authService.Authenticate(model.Email, model.Password);

			return Ok(result);
		}

	}
}
