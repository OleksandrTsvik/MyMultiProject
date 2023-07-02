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
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Photo>()
            .HasOne(photo => photo.AppUser)
            .WithMany(user => user.Photos)
            .OnDelete(DeleteBehavior.Cascade);
    }
}