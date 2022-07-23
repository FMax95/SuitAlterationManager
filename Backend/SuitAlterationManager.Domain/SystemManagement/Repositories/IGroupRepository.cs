using SuitAlterationManager.Domain.Base.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.SystemManagement.Repositories
{
    public interface IGroupRepository : IRepository<Group, GroupID>
	{
		Group GetFromName(string groupName);
		ValueTask<Group> GetFromNameAsync(string groupName);
		Task<bool> ExistsWithNameAsync(string name, GroupID differentThan = null);
		Task<List<Group>> GetManyAsync(IEnumerable<GroupID> idGroupList);
	}
}
