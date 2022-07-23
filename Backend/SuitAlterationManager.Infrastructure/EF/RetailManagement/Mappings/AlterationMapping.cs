using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.ValueObjects;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
    public class AlterationMapping : IEntityTypeConfiguration<Alteration>
	{
		public void Configure(EntityTypeBuilder<Alteration> builder)
		{
			builder.ToTable("Alteration", "Retail");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				.HasConversion(x => x.Value, x => new AlterationID(x));

			builder.Property(p => p.Type)
						.HasEnumToStringConversion();
			
			builder.Property(p => p.Status)
						.HasEnumToStringConversion();

			builder.Property(p => p.Direction)
						.HasEnumToStringConversion();

			builder.Property(p => p.DatabaseVersion)
				.IsRequired()
				.IsConcurrencyToken()
				.ValueGeneratedOnAddOrUpdate();
        }
	}
}
