namespace Application.GrammarRules.DTOs;

public class CreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Language { get; set; }
#nullable enable
    public string? Status { get; set; }
#nullable disable
}
