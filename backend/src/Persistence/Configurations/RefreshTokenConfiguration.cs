using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(refreshToken => refreshToken.Id);

        builder
            .Property(refreshToken => refreshToken.Token)
            .IsRequired();

        builder
            .HasIndex(refreshToken => refreshToken.Token)
            .IsUnique();

        builder
            .Property(refreshToken => refreshToken.ExpiresOnUtc)
            .IsRequired();

        builder
            .HasOne(refreshToken => refreshToken.User)
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
