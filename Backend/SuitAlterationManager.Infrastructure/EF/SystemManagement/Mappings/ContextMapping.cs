using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
    public class ContextMapping : IEntityTypeConfiguration<Context>
	{

		public void Configure(EntityTypeBuilder<Context> builder)
		{
			builder.ToTable("Context", "System");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				   .HasConversion(x => x.Value, x => new ContextID(x));

			builder.Property(p => p.DatabaseVersion)
				   .IsRequired()
				   .IsConcurrencyToken()
				   .ValueGeneratedOnAddOrUpdate();
		}
	}
}
