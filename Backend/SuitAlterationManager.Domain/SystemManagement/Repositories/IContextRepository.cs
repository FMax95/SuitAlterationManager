using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Repositories
{
    public interface IContextRepository : IRepository<Context, ContextID>
	{
		Context GetFromName(string groupName);
		ValueTask<Context> GetFromNameAsync(string groupName);
		Task<bool> ExistsWithNameAsync(string name, ContextID differentThan = null);
	}
}
