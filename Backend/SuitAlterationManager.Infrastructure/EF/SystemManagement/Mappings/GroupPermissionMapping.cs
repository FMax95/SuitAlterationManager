using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
	public class GroupPermissionMapping : IEntityTypeConfiguration<GroupPermission>
	{
		public void Configure(EntityTypeBuilder<GroupPermission> builder)
		{
			builder.ToTable("GroupPermission", "System");
			builder.HasKey(x => new { x.IdGroup, x.IdAction});

			builder.Property(p => p.IdGroup)
				   .HasConversion(x => x.Value, x => new GroupID(x));

			builder.Property(p => p.IdAction)
				   .HasConversion(x => x.Value, x => new ActionID(x));
		}
	}
}
