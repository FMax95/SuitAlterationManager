using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.RetailManagement.Models;
using SuitAlterationManager.Api.Client.SystemManagement.Queries;
using SuitAlterationManager.Domain.RetailManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Extensions;
using SuitAlterationManager.Infrastructure.EF;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/alterations")]
    public class AlterationController : BaseController
    {
        private readonly DbContext context;
        private readonly IAlterationService alterationService;
        private readonly IMapper mapper;

        public AlterationController(DbContext context, IAlterationService alterationService, IMapper mapper)
        {
            this.context = context;
            this.alterationService = alterationService;
            this.mapper = mapper;
        }

        
        [HttpPost("")]
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