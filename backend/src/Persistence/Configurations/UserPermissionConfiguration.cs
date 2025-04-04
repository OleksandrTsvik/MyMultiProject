using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations;

public sealed class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(userPermission => userPermission.Id);

        builder
            .Property(userPermission => userPermission.Name)
            .IsRequired()
            .HasConversion(new EnumToStringConverter<UserPermissionType>())
            .HasMaxLength(UserPermissionRules.MaxNameLength);

        builder
            .HasIndex(userPermission => userPermission.Name)
            .IsUnique();

        builder
            .HasMany(userPermission => userPermission.Roles)
            .WithMany(userRole => userRole.Permissions);
    }
}
