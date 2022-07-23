using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Api.Client.RetailManagement.Models;
using SuitAlterationManager.Api.Client.SystemManagement.Queries;
using SuitAlterationManager.Domain.RetailManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Infrastructure.EF;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.SystemManagement
{
    [Route("api/alterations")]
    public class AlterationController : BaseController
    {
        private readonly DbContext context;
        private readonly IAlterationService alterationService;
        private readonly IMapper mapper;
        private readonly IAlterationQueries alterationQueries;

        public AlterationController(DbContext context, IAlterationService alterationService, IMapper mapper, IAlterationQueries alterationQueries)
        {
            this.context = context;
            this.alterationService = alterationService;
            this.mapper = mapper;
            this.alterationQueries = alterationQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await alterationQueries.GetAlterationsAsync();
            return Ok(result);
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