using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.ValueObjects;

namespace SuitAlterationManager.Domain.RetailManagement.Repositories
{
    public interface IAlterationRepository : IRepository<Alteration, AlterationID>
	{
    }
}
