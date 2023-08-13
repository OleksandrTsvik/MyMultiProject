using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder
            .HasOne(photo => photo.AppUser)
            .WithMany(user => user.Photos)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
