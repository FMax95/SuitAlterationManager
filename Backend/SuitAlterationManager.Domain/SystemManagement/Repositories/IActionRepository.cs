using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Repositories
{
    public interface IActionRepository : IRepository<Action, ActionID>
	{
		Action GetFromNameAndContext(string actionName, ContextID idContext);
		ValueTask<Action> GetFromNameAndContextAsync(string actionName, ContextID idContext);
		Task<bool> ExistsWithNameAndContextAsync(string actionName, ContextID idContext, ActionID differentThan = null);
	}
}
