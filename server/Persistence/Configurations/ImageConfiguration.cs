using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder
            .HasIndex(image => image.Name)
            .IsUnique();

        builder
            .HasOne(image => image.AppUser)
            .WithMany(user => user.Images)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
