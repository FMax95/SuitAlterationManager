using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuitAlterationManager.Infrastructure.EF.SystemManagement.Mappings
{
	public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken", "System");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)                   
                   .HasConversion(x => x.Value, x => new RefreshTokenID(x));
            
            builder.Property(p => p.IdUser)               
                .HasConversion(x => x.Value, x => new UserID(x));
        }
    }
}