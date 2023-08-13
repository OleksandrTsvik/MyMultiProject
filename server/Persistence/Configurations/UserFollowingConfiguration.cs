using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowing>
{
    public void Configure(EntityTypeBuilder<UserFollowing> builder)
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
    }
}
