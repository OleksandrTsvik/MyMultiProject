using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.HasKey(emailVerificationToken => emailVerificationToken.Id);

        builder
            .Property(emailVerificationToken => emailVerificationToken.Token)
            .IsRequired();

        builder
            .HasIndex(emailVerificationToken => emailVerificationToken.Token)
            .IsUnique();

        builder
            .Property(emailVerificationToken => emailVerificationToken.ExpiresOnUtc)
            .IsRequired();

        builder
            .HasOne(emailVerificationToken => emailVerificationToken.User)
            .WithMany()
            .HasForeignKey(emailVerificationToken => emailVerificationToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
