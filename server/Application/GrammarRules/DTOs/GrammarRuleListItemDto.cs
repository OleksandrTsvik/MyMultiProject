namespace Application.GrammarRules.DTOs;

public class GrammarRuleListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
#nullable enable
    public string? Status { get; set; }
#nullable disable
    public int Position { get; set; }
    public DateTime DateCreation { get; set; }
}
