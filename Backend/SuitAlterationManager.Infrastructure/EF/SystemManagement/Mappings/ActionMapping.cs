using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
    public class ActionMapping : IEntityTypeConfiguration<Action>
	{

		public void Configure(EntityTypeBuilder<Action> builder)
		{
			builder.ToTable("Action", "System");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				   .HasConversion(x => x.Value, x => new ActionID(x));

			builder.Property(p => p.IdContext)
				   .HasConversion(x => x.Value, x => new ContextID(x));

			builder.Property(p => p.DatabaseVersion)
				   .IsRequired()
				   .IsConcurrencyToken()
				   .ValueGeneratedOnAddOrUpdate();
		}
	}
}
