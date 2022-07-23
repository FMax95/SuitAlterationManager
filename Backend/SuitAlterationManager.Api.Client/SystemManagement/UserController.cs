using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.SystemManagement.Queries;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Infrastructure.EF;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly DbContext context;
        private readonly IUserQueries queries;

        public UserController(DbContext context, IUserQueries queries)
        {
            this.context = context;
            this.queries = queries;
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> Get([FromRoute] Guid idUser)
        {
            var user = await queries.GetUserAsync(idUser);

            return Ok(user);
        }

        [HttpPut("{idUser}")]
        public async Task<IActionResult> Update([FromRoute] Guid idUser)
        {
            return await context.Execute(async () =>
            {
                return Ok();
            });
        }
    }
}