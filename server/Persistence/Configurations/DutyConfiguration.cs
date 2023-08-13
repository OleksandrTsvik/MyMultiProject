using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class DutyConfiguration : IEntityTypeConfiguration<Duty>
{
    public void Configure(EntityTypeBuilder<Duty> builder)
    {
        builder
            .HasOne(duty => duty.AppUser)
            .WithMany(user => user.Duties)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
