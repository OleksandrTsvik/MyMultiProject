using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.HasKey(passwordResetToken => passwordResetToken.Id);

        builder
            .Property(passwordResetToken => passwordResetToken.Token)
            .IsRequired();

        builder
            .HasIndex(passwordResetToken => passwordResetToken.Token)
            .IsUnique();

        builder
            .Property(passwordResetToken => passwordResetToken.ExpiresOnUtc)
            .IsRequired();

        builder
            .HasOne(passwordResetToken => passwordResetToken.User)
            .WithMany()
            .HasForeignKey(passwordResetToken => passwordResetToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
