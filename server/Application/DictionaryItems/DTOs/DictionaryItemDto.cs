namespace Application.DictionaryItems.DTOs;

public class DictionaryItemDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Translation { get; set; }
    public string Status { get; set; }
    public DateTime DateCreation { get; set; }
    public int Position { get; set; }
}
