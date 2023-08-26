using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class GrammarRuleItemConfiguration : IEntityTypeConfiguration<GrammarRuleItem>
{
    public void Configure(EntityTypeBuilder<GrammarRuleItem> builder)
    {
        builder.HasKey(grammarRuleItem => new
        {
            grammarRuleItem.GrammarRuleId,
            grammarRuleItem.DictionaryItemId
        });

        builder
            .HasOne(grammarRuleItem => grammarRuleItem.GrammarRule)
            .WithMany(grammarRule => grammarRule.Items)
            .HasForeignKey(grammarRuleItem => grammarRuleItem.GrammarRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(grammarRuleItem => grammarRuleItem.DictionaryItem)
            .WithMany(dictionaryItem => dictionaryItem.Rules)
            .HasForeignKey(grammarRuleItem => grammarRuleItem.DictionaryItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
