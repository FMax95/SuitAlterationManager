using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.ValueObjects;
using SuitAlterationManager.Domain.RetailManagement.DTO;
using SuitAlterationManager.Domain.RetailManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Services
{
    public class AlterationService : IAlterationService
    {
        private readonly IAlterationRepository alterationRepository;

        public AlterationService(IAlterationRepository alterationRepository)
        {
            this.alterationRepository = alterationRepository;
        }

        public async Task CreateAlterationAsync(CreateAlterationDTO input)
        {
            var entity = Alteration.Create(customerEmail: input.CustomerEmail,
                                           alterationType: input.AlterationType,
                                           alterationTypeDirection: input.AlterationDirection,
                                           measure: input.Measure);
            
            await this.alterationRepository.AddAsync(entity);
        }


        public async Task StartAlterationAsync(Guid idAlteration)
        {
            var entity = await this.alterationRepository.GetAsync(new AlterationID(idAlteration));
            entity.StartAlteration();
        }

        public async Task FinishAlterationAsync(Guid idAlteration)
        {
            var entity = await this.alterationRepository.GetAsync(new AlterationID(idAlteration));
            entity.FinishAlteration();
        }

    }
}
