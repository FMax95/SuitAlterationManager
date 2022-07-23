using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.CMS.Filters;
using SuitAlterationManager.Api.CMS.SystemManagement.Models;
using SuitAlterationManager.Api.CMS.SystemManagement.Queries;
using SuitAlterationManager.Api.CMS.SystemManagement.Responses;
using SuitAlterationManager.Domain.Constants;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Extensions.Attributes;
using SuitAlterationManager.Infrastructure.EF;
using System;
using System.Threading.Tasks;
using ApplicationService = SuitAlterationManager.Api.CMS.SystemManagement.Services;
using DomainService = SuitAlterationManager.Domain.SystemManagement.Services;

namespace SuitAlterationManager.Api.CMS.SystemManagement
{
    [Route("api/users")]
    [Authorize]
    [TypeFilter(typeof(AllowExceptionAttribute))]
    public class UserController : BaseController
    {
        private readonly DbContext context;
        private readonly IUserQueries queries;
        private readonly ApplicationService.Interfaces.IUserService userCmsService;
        private readonly DomainService.Interfaces.IUserService userService;

        public UserController(DbContext context, IUserQueries queries, ApplicationService.Interfaces.IUserService userCmsService, DomainService.Interfaces.IUserService userService)
        {
            this.context = context;
            this.queries = queries;
            this.userCmsService = userCmsService;
            this.userService = userService;
        }

        [HttpGet]
        [TypeFilter(typeof(ValidatePermissionsFilter), Arguments = new object[] { ActionValue.Read })]
        public async Task<IActionResult> GetAll()
        {
            var result = await queries.GetUsersAsync();

            return Ok(result);
        }

        [HttpGet("{idUser}")]
        [TypeFilter(typeof(ValidatePermissionsFilter), Arguments = new object[] { ActionValue.Read })]
        public async Task<IActionResult> Get([FromRoute] Guid idUser)
        {
            var user = await queries.GetUserAsync(idUser);

            return Ok(user);
        }

        [HttpPost]
        [TypeFilter(typeof(ValidatePermissionsFilter), Arguments = new object[] { ActionValue.Create })]
        public async Task<IActionResult> Create([FromBody] CreateUser model)
        {
            return await context.Execute(async () =>
            {
                var user = await userCmsService.CreateUser(email: model.Email,
                                             password: model.Password,
                                             groups: model.IdGroupList,
                                             birthDate: model.BirthDate == null ? null : Convert.ToDateTime(model.BirthDate),
                                             firstName: model.FirstName,
                                             lastName: model.LastName,
                                             image: model.Image
                                              );

                return Created(new UserResponse()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsEnabled = user.IsEnabled,
                    Id = user.Id,
                    Image = user.Image
                });
            });
        }

        [HttpPut("{idUser}")]
        public async Task<IActionResult> Update([FromRoute] Guid idUser, [FromBody] UpdateUser model)
        {
            return await context.Execute(async () =>
            {
                await userCmsService.UpdateUser(userId: new UserID(idUser),
                                             email: model.Email,
                                             groups: model.IdGroupList,
                                             birthDate: model.BirthDate == null ? null : Convert.ToDateTime(model.BirthDate),
                                             firstName: model.FirstName,
                                             lastName: model.LastName,
                                             image: model.Image,
                                             isEnabled: model.IsEnabled
                                              );
                return Ok();
            });
        }


        [HttpPatch("{idUser}/disable")]
        public async Task<IActionResult> Disable([FromRoute] Guid idUser) => await context.Execute(async () =>
        {
            await userService.DisableUser(new UserID(idUser));
            return Ok();
        });
    }
}