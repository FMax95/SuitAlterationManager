using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Repositories
{
    public class ContextRepository : BaseRepository<Context, ContextID>, IContextRepository
	{
		private readonly DbSet<Context> contexts;
		public ContextRepository(DbContext context) : base(context)
		{
			contexts = context.Set<Context>();
		}

		public override Context Get(ContextID id)
		{
			var context = contexts.Find(id);

			if (context is null)
				return null;

			return context;
		}

		public override async ValueTask<Context> GetAsync(ContextID id)
		{
			var context = await contexts.FindAsync(id);

			if (context is null)
				return null;

			return context;
		}

		public Context GetFromName(string contextName)
		{
			return contexts.FirstOrDefault(g => g.Name == contextName);
		}

		public async ValueTask<Context> GetFromNameAsync(string contextName)
		{
			return await contexts.FirstOrDefaultAsync(g => g.Name == contextName);
		}

		public Task<bool> ExistsWithNameAsync(string name, ContextID differentThan = null) =>
			contexts.AnyAsync(g => g.Name == name && g.Id != differentThan);

	}
}
