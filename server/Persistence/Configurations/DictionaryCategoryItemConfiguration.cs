using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class DictionaryCategoryItemConfiguration : IEntityTypeConfiguration<DictionaryCategoryItem>
{
    public void Configure(EntityTypeBuilder<DictionaryCategoryItem> builder)
    {
        builder.HasKey(dictionaryCategoryItem => new
        {
            dictionaryCategoryItem.DictionaryCategoryId,
            dictionaryCategoryItem.DictionaryItemId
        });

        builder
            .HasOne(dictionaryCategoryItem => dictionaryCategoryItem.DictionaryCategory)
            .WithMany(dictionaryCategory => dictionaryCategory.Items)
            .HasForeignKey(dictionaryCategoryItem => dictionaryCategoryItem.DictionaryCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(dictionaryCategoryItem => dictionaryCategoryItem.DictionaryItem)
            .WithMany(dictionaryItem => dictionaryItem.Categories)
            .HasForeignKey(dictionaryCategoryItem => dictionaryCategoryItem.DictionaryItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
