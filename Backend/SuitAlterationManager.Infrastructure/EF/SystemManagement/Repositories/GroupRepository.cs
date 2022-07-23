using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Repositories
{
	public class GroupRepository : BaseRepository<Group, GroupID>, IGroupRepository
	{
		private readonly DbSet<Group> groups;
		public GroupRepository(DbContext context) : base(context)
		{
			groups = context.Set<Group>();
		}

		public override Group Get(GroupID id)
		{
			var group = groups.Find(id);

			if (group is null)
				return null;

			return group;
		}

		public override async ValueTask<Group> GetAsync(GroupID id)
		{
			var group = await groups.FindAsync(id);

			if (group is null)
				return null;

			return group;
		}

		public Task<bool> ExistsWithNameAsync(string name, GroupID differentThan = null) =>
			groups.AnyAsync(g => g.Name == name && g.Id != differentThan);

		public Task<List<Group>> GetManyAsync(IEnumerable<GroupID> idGroupList) =>
			groups.Where(g => idGroupList.Any(i => i == g.Id)).ToListAsync();

		public Group GetFromName(string groupName)
		{
			return groups.FirstOrDefault(g => g.Name == groupName);
		}

		public async ValueTask<Group> GetFromNameAsync(string groupName)
		{
			return await groups.FirstOrDefaultAsync(g => g.Name == groupName);
		}
	}
}
