using Newtonsoft.Json;
using SuitAlterationManager.Api.Client.AlterationManagement.Queries;
using SuitAlterationManager.Api.Client.RetailManagement.Services.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Infrastructure.MessageDispatchers;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.RetailManagement.Services
{
    public class AlterationApplicationService : IAlterationApplicationService
    {
        private readonly IAlterationService alterationService;
        private readonly IAlterationQueries alterationQueries;
        private readonly IMessageDispatcherService messageDispatcher;
        public AlterationApplicationService(IAlterationService alterationService, IAlterationQueries alterationQueries, IMessageDispatcherService messageDispatcher)
        {
            this.alterationService = alterationService;
            this.alterationQueries = alterationQueries;
            this.messageDispatcher = messageDispatcher;
        }
        /// <summary>
        /// Ends an alteration and send an AlterationFinished message
        /// </summary>
        /// <param name="idAlteration"></param>
        /// <returns></returns>
        public async Task FinishAlteration(Guid idAlteration)
        {
            await alterationService.FinishAlterationAsync(idAlteration);
            var customerEmail = await this.alterationQueries.FindAlterationMailAsync(idAlteration);
            await messageDispatcher.SendMessageAsync("AlterationFinished", JsonConvert.SerializeObject(new
            {
                idAlteration = idAlteration,
                customerEmail = customerEmail
            }));
        }
    }
}
