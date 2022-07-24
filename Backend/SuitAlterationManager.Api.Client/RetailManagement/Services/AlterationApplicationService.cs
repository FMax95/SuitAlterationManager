using Newtonsoft.Json;
using SuitAlterationManager.Api.Client.AlterationManagement.Queries;
using SuitAlterationManager.Api.Client.RetailManagement.Services.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.RetailManagement.Services
{
    public class AlterationApplicationService : IAlterationApplicationService
    {
        private readonly IAlterationService alterationService;
        private readonly IAlterationQueries alterationQueries;
        public AlterationApplicationService(IAlterationService alterationService, IAlterationQueries alterationQueries)
        {
            this.alterationService = alterationService;
            this.alterationQueries = alterationQueries; 
        }

        public async Task FinishAlteration(Guid idAlteration)
        {
            await alterationService.FinishAlterationAsync(idAlteration);
            var alteration = await this.alterationQueries.FindAlterationAsync(idAlteration);
            await AzureServiceBusDispatcher.SendMessageAsync("AlterationFinished", JsonConvert.SerializeObject(alteration));
        }
    }
}
