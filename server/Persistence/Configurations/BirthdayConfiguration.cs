using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class BirthdayConfiguration : IEntityTypeConfiguration<Birthday>
{
    public void Configure(EntityTypeBuilder<Birthday> builder)
    {
        builder
            .HasIndex(birthday => birthday.FullName)
            .IsUnique();

        builder
            .HasOne(birthday => birthday.AppUser)
            .WithMany(user => user.Birthdays)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
