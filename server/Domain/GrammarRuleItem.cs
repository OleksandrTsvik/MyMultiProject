namespace Domain;

public class GrammarRuleItem
{
    public Guid GrammarRuleId { get; set; }
    public Guid DictionaryItemId { get; set; }
    public int Position { get; set; }

    public GrammarRule GrammarRule { get; set; }
    public DictionaryItem DictionaryItem { get; set; }
}
