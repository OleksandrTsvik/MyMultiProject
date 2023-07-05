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
    public DbSet<UserFollowing> UserFollowings { get; set; }

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

        builder.Entity<UserFollowing>(builder =>
        {
            builder.HasKey(user => new { user.ObserverId, user.TargetId });

            builder
                .HasOne(userFollowing => userFollowing.Observer)
                .WithMany(appUser => appUser.Followings)
                .HasForeignKey(userFollowing => userFollowing.ObserverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(userFollowing => userFollowing.Target)
                .WithMany(appUser => appUser.Followers)
                .HasForeignKey(userFollowing => userFollowing.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}