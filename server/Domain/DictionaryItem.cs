namespace Domain;

public class DictionaryItem
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Translation { get; set; }
#nullable enable
    public string? Status { get; set; }
#nullable disable
    public DateTime DateCreation { get; set; }

    public ICollection<DictionaryCategoryItem> Categories { get; set; }
    public ICollection<GrammarRuleItem> Rules { get; set; }
    public AppUser AppUser { get; set; }

    public DictionaryItem()
    {
        Categories = new List<DictionaryCategoryItem>();
        Rules = new List<GrammarRuleItem>();
    }
}
