using SuitAlterationManager.Api.Client.Base.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client.RetailManagement.Services.Interfaces
{
    public interface IAlterationApplicationService : IBaseApplicationService
    {
        Task FinishAlteration(Guid idAlteration);
    }
}
