namespace Domain;

public class GrammarRule
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
#nullable enable
    public string? Status { get; set; }
#nullable disable
    public int Position { get; set; }
    public DateTime DateCreation { get; set; }

    public ICollection<GrammarRuleItem> Items { get; set; }
    public AppUser AppUser { get; set; }

    public GrammarRule()
    {
        Items = new List<GrammarRuleItem>();
    }
}
