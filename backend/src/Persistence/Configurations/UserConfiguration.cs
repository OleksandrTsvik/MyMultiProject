using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder
            .Property(user => user.UserName)
            .IsRequired()
            .HasMaxLength(UserRules.MaxUserNameLength);

        builder
            .HasIndex(user => user.UserName)
            .IsUnique();

        builder
            .Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(UserRules.MaxEmailLength);

        builder
            .HasIndex(user => user.Email)
            .IsUnique();

        builder
            .Property(user => user.EmailVerified)
            .IsRequired();

        builder
            .Property(user => user.PasswordHash)
            .IsRequired(false);

        builder
            .Property(user => user.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(user => user.DeletedOnUtc)
            .IsRequired(false);

        builder
            .Property(user => user.DeletedBy)
            .IsRequired(false);

        builder.Ignore(user => user.IsDeleted);
        builder.Ignore(user => user.Permissions);
    }
}
