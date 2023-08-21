using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DbSet<Duty> Duties { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<UserFollowing> UserFollowings { get; set; }
    public DbSet<Image> Images { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.ApplyConfiguration(new DutyConfiguration());
        builder.ApplyConfiguration(new PhotoConfiguration());
        builder.ApplyConfiguration(new UserFollowingConfiguration());
        builder.ApplyConfiguration(new ImageConfiguration());
    }
}
