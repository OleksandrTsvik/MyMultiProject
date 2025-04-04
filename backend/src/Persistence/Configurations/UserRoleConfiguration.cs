using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(userRole => userRole.Id);

        builder
            .Property(userRole => userRole.Name)
            .IsRequired()
            .HasMaxLength(UserRoleRules.MaxNameLength);

        builder
            .HasIndex(userRole => userRole.Name)
            .IsUnique();

        builder
            .HasMany(userRole => userRole.Users)
            .WithMany(user => user.Roles);
    }
}
