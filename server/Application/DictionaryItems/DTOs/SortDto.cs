namespace Application.DictionaryItems.DTOs;

public class SortDto
{
    public Guid ItemId { get; set; }
    public Guid CategoryId { get; set; }
    public int Position { get; set; }
}
