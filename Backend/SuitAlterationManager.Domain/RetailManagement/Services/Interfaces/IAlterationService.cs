using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.RetailManagement.DTO;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Services.Interfaces
{
    public interface IAlterationService : IBaseService
    {
        Task CreateAlterationAsync(CreateAlterationDTO input);
        Task FinishAlterationAsync(Guid idAlteration);
        Task StartAlterationAsync(Guid idAlteration);
    }
}
