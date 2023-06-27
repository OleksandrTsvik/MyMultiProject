using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Duty> Duties { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Duty>()
            .HasOne(duty => duty.AppUser)
            .WithMany(user => user.Duties)
            .HasForeignKey(duty => duty.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Duty>()
            .Property(duty => duty.AppUserId)
            .IsRequired();
    }
}