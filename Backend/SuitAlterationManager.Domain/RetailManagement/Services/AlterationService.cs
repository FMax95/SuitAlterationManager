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
        /// <summary>
        /// Creates a new Alteration
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateAlterationAsync(CreateAlterationDTO input)
        {
            var entity = Alteration.Create(customerEmail: input.CustomerEmail,
                                           alterationType: input.AlterationType,
                                           alterationTypeDirection: input.AlterationDirection,
                                           measure: input.Measure);
            
            await this.alterationRepository.AddAsync(entity);
        }

        /// <summary>
        /// Marks the alteration as Started
        /// </summary>
        /// <param name="idAlteration"></param>
        /// <returns></returns>
        public async Task StartAlterationAsync(Guid idAlteration)
        {
            var entity = await this.alterationRepository.GetAsync(new AlterationID(idAlteration));
            entity.StartAlteration();
        }
        /// <summary>
        /// Marks the alteration as Paid
        /// </summary>
        /// <param name="idAlteration"></param>
        /// <returns></returns>
        public async Task PayAlterationAsync(Guid idAlteration)
        {
            var entity = await this.alterationRepository.GetAsync(new AlterationID(idAlteration));
            entity.PayAlteration();
        }
        /// <summary>
        /// Marks the alteration as Done
        /// </summary>
        /// <param name="idAlteration"></param>
        /// <returns></returns>
        public async Task FinishAlterationAsync(Guid idAlteration)
        {
            var entity = await this.alterationRepository.GetAsync(new AlterationID(idAlteration));
            entity.FinishAlteration();
        }

    }
}
