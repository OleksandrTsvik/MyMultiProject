namespace Application.DictionaryCategories.DTOs;

public class DictionaryCategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
    public int Position { get; set; }
    public DateTime DateCreation { get; set; }
    public int CountItems { get; set; }
}
