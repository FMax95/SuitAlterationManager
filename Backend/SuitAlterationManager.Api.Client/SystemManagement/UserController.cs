using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.SystemManagement.Models;
using SuitAlterationManager.Api.Client.SystemManagement.Queries;
using SuitAlterationManager.Api.Client.SystemManagement.Responses;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Extensions.Attributes;
using SuitAlterationManager.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationService = SuitAlterationManager.Api.Client.SystemManagement.Services;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly DbContext context;
        private readonly IUserQueries queries;
        private readonly ApplicationService.Interfaces.IUserService userClientService;

        public UserController(DbContext context, IUserQueries queries, ApplicationService.Interfaces.IUserService userClientService)
        {
            this.context = context;
            this.queries = queries;
            this.userClientService = userClientService;
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> Get([FromRoute] Guid idUser)
        {
            var user = await queries.GetUserAsync(idUser);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUser model)
        {
            return await context.Execute(async () =>
            {
                var user = await userClientService.CreateUser(email: model.Email,
                                             password: model.Password,
                                             birthDate: model.BirthDate,
                                             firstName: model.FirstName,
                                             lastName: model.LastName
                                              );

                return Created(new UsersResponse()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsEnabled = user.IsEnabled,
                    Id = user.Id
                });
            });
        }

        [HttpPut("{idUser}")]
        public async Task<IActionResult> Update([FromRoute] Guid idUser, [FromBody] UpdateUser model)
        {
            return await context.Execute(async () =>
            {
                await userClientService.UpdateUser(userId: new UserID(idUser),
                                             email: model.Email,
                                             isEnabled: true,
                                             birthDate: model.BirthDate,
                                             firstName: model.FirstName,
                                             lastName: model.LastName
                                              );
                return Ok();
            });
        }
    }
}