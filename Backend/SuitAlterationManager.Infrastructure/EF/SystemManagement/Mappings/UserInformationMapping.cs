using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
	public class UserInformationMapping : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
            builder.ToTable("UserInformation", "System");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .HasColumnName("IdUser")
                   .HasConversion(x => x.Value, x => new UserID(x));
        }
    }
}
