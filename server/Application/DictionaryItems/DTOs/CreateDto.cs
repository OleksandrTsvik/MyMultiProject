namespace Application.DictionaryItems.DTOs;

public class CreateDto
{
    public Guid CategoryId { get; set; }
    public string Text { get; set; }
    public string Translation { get; set; }
    public string Status { get; set; }
}
