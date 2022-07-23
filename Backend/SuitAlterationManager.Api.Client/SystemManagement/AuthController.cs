using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Api.Client.SystemManagement.Models;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/auth")]
	public class AuthController : BaseController
	{
		private readonly IAuthService authService;
		private readonly DbContext context;

		public AuthController(
			IAuthService authService,
			DbContext context)
		{
			this.authService = authService;
			this.context = context;
		}

		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] AuthModel model)
		{
			var result = await authService.Authenticate(model.Email, model.Password);

			return Ok(result);
		}

	}
}
