using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
	public class UserMapping : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("User", "System");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				.HasConversion(x => x.Value, x => new UserID(x));

			builder.Property(p => p.DatabaseVersion)
				.IsRequired()
				.IsConcurrencyToken()
				.ValueGeneratedOnAddOrUpdate();
        }
	}
}
