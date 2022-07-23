using SuitAlterationManager.Domain.RetailManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.ValueObjects;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Repositories
{
    public class AlterationRepository : BaseRepository<Alteration, AlterationID>, IAlterationRepository
	{
		private readonly DbSet<Alteration> alterations;
		public AlterationRepository(DbContext context) : base(context)
		{
			alterations = context.Set<Alteration>();
		}

	}
}
