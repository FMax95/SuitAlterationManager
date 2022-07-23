using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.Services;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SuitAlterationManager.Domain.SystemManagement.Services.Auth;

namespace SuitAlterationManager.Extensions.Middlewares
{
    public class AuthMiddleware
	{
		private readonly RequestDelegate next;

		public AuthMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context, IUserRepository users, IGroupRepository groups, IAuthService authService)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (token != null)
			{
				var jwtToken = authService.ValidateJwtToken(token);
				if (jwtToken != null)
				{
					var sub = jwtToken.Claims.Single(x => x.Type == "sub").Value;
					var idUser = new UserID(new Guid(sub));
					var user = await users.GetAsync(idUser);
					if (user is null || !user.IsEnabled)
					{
						context.User = new ClaimsPrincipal(new ClaimsIdentity());
						// DEBUG ONLY
						//context.Items["LoggedUserId"] = new UserID(new Guid("00000000-0000-0000-0000-000000000000"));
					}
					else
					{
						var userGroups = await groups.GetManyAsync(user.Groups.Select(g => g.IdGroup));
						var identity = authService.CreateIdentity(user, userGroups);
						context.User.AddIdentity(identity);
						context.Items["LoggedUserId"] = idUser;
					}
				}
			}
			// DEBUG ONLY
			//context.Items["LoggedUserId"] = new UserID(new Guid("00000000-0000-0000-0000-000000000000"));

			await next(context);
		}
	}
}
