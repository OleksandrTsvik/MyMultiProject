using Domain.Birthdays;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class BirthdayConfiguration : IEntityTypeConfiguration<Birthday>
{
    public void Configure(EntityTypeBuilder<Birthday> builder)
    {
        builder.HasKey(birthday => birthday.Id);

        builder
            .Property(birthday => birthday.FullName)
            .IsRequired()
            .HasMaxLength(BirthdayRules.MaxFullNameLength);

        builder
            .HasIndex(birthday => birthday.FullName)
            .IsUnique();

        builder
            .Property(birthday => birthday.Date)
            .IsRequired();

        builder
            .Property(birthday => birthday.Note)
            .IsRequired(false);

        builder
            .HasOne(birthday => birthday.User)
            .WithMany()
            .HasForeignKey(birthday => birthday.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
