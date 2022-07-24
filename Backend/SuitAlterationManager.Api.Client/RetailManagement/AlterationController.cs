using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.AlterationManagement.Queries;
using SuitAlterationManager.Api.Client.Base;
using SuitAlterationManager.Api.Client.RetailManagement.Models;
using SuitAlterationManager.Api.Client.RetailManagement.Services.Interfaces;
using SuitAlterationManager.Domain.RetailManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Infrastructure.EF;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Authorize]
    [Route("api/alterations")]
    public class AlterationController : BaseController
    {
        private readonly DbContext context;
        private readonly IAlterationService alterationService;
        private readonly IAlterationApplicationService alterationApplicationService;
        private readonly IMapper mapper;
        private readonly IAlterationQueries alterationQueries;

        public AlterationController(DbContext context, IAlterationService alterationService, IMapper mapper, IAlterationQueries alterationQueries, IAlterationApplicationService alterationApplicationService)
        {
            this.context = context;
            this.alterationService = alterationService;
            this.mapper = mapper;
            this.alterationQueries = alterationQueries;
            this.alterationApplicationService = alterationApplicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await alterationQueries.GetAlterationsAsync();
            return Ok(result);
        }

        [HttpPut("{idAlteration}/start")]
        public async Task<IActionResult> Start([FromRoute] Guid idAlteration)
        {
            return await context.Execute(async () =>
            {
                await alterationService.StartAlterationAsync(idAlteration);
                return Ok();
            });
        }

        [HttpPut("{idAlteration}/finish")]
        public async Task<IActionResult> Finish([FromRoute] Guid idAlteration)
        {
            return await context.Execute(async () =>
            {
                await alterationApplicationService.FinishAlteration(idAlteration);
                return Ok();
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAlterationModel model)
        {
            return await context.Execute(async () =>
            {
                await alterationService.CreateAlterationAsync(mapper.Map<CreateAlterationDTO>(model));
                return Ok();
            });
        }
    }
}