using SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings;
using Microsoft.EntityFrameworkCore;

namespace SuitAlterationManager.Infrastructure.EF
{
    public class SuitAlterationManagerContext : DbContext
	{
		public SuitAlterationManagerContext(DbContextOptions<SuitAlterationManagerContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AlterationMapping());
		}
	}
}
