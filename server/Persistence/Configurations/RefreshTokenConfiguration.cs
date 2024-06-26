using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .HasOne(refreshToken => refreshToken.AppUser)
            .WithMany(user => user.RefreshTokens)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
