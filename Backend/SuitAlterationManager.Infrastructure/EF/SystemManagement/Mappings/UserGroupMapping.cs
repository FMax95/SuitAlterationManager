using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
	public class UserGroupMapping : IEntityTypeConfiguration<UserGroup>
	{
		public void Configure(EntityTypeBuilder<UserGroup> builder)
		{
			builder.ToTable("UserGroup", "System");
			builder.HasKey(x => new { x.IdUser, x.IdGroup });

			builder.Property(p => p.IdUser)
				   .HasConversion(x => x.Value, x => new UserID(x));

			builder.Property(p => p.IdGroup)
				   .HasConversion(x => x.Value, x => new GroupID(x));
		}
	}
}
