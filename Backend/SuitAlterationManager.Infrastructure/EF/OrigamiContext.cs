using SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings;
using Microsoft.EntityFrameworkCore;

namespace SuitAlterationManager.Infrastructure.EF
{
    public class SuitAlterationManagerContext : DbContext
	{
		public SuitAlterationManagerContext(DbContextOptions<SuitAlterationManagerContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserMapping());
			modelBuilder.ApplyConfiguration(new UserGroupMapping());
			modelBuilder.ApplyConfiguration(new UserInformationMapping());
			modelBuilder.ApplyConfiguration(new RefreshTokenMapping());
			modelBuilder.ApplyConfiguration(new GroupMapping());
			modelBuilder.ApplyConfiguration(new GroupPermissionMapping());
			modelBuilder.ApplyConfiguration(new ContextMapping());
			modelBuilder.ApplyConfiguration(new ActionMapping());
		}
	}
}
