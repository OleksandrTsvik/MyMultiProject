namespace Domain;

public class DictionaryCategory
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Language { get; set; }
    public int Position { get; set; }
    public DateTime DateCreation { get; set; }

    public ICollection<DictionaryCategoryItem> Items { get; set; }
    public AppUser AppUser { get; set; }

    public DictionaryCategory()
    {
        Items = new List<DictionaryCategoryItem>();
    }
}
