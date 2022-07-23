using SuitAlterationManager.Domain.RetailManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;

namespace SuitAlterationManager.Domain.SystemManagement.Services
{
    public class AlterationService : IAlterationService
    {
        private readonly IAlterationRepository alterationRepository;

        public AlterationService(IAlterationRepository alterationRepository)
        {
            this.alterationRepository = alterationRepository;
        }

    }
}
