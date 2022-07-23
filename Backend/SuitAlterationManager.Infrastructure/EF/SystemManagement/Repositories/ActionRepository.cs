using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Repositories
{
    public class ActionRepository : BaseRepository<Action, ActionID>, IActionRepository
	{
		private readonly DbSet<Action> actions;
		public ActionRepository(DbContext context) : base(context)
		{
			actions = context.Set<Action>();
		}

		public override Action Get(ActionID id)
		{
			var action = actions.Find(id);

			if (action is null)
				return null;

			return action;
		}

		public override async ValueTask<Action> GetAsync(ActionID id)
		{
			var action = await actions.FindAsync(id);

			if (action is null)
				return null;

			return action;
		}

		public Action GetFromNameAndContext(string actionName, ContextID idContext)
		{
			return actions.FirstOrDefault(x => x.Name == actionName && x.IdContext == idContext);
		}

		public async ValueTask<Action> GetFromNameAndContextAsync(string actionName, ContextID idContext)
		{
			return await actions.FirstOrDefaultAsync(x => x.Name == actionName && x.IdContext == idContext);
		}

		public Task<bool> ExistsWithNameAndContextAsync(string name, ContextID idContext, ActionID differentThan = null) =>
			actions.AnyAsync(x => x.Name == name && x.IdContext == idContext && x.Id != differentThan);
	}
}
