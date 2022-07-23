using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
	public class GroupMapping : IEntityTypeConfiguration<Group>
	{

		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.ToTable("Group", "System");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				   .HasConversion(x => x.Value, x => new GroupID(x));

			builder.HasMany(p => p.Permissions)
				.WithOne()
				.HasForeignKey(p => p.IdGroup)
				.IsRequired();

			builder.Property(p => p.DatabaseVersion)
				   .IsRequired()
				   .IsConcurrencyToken()
				   .ValueGeneratedOnAddOrUpdate();
		}
	}
}
