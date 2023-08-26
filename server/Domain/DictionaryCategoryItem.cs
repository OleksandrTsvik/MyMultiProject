namespace Domain;

public class DictionaryCategoryItem
{
    public Guid DictionaryCategoryId { get; set; }
    public Guid DictionaryItemId { get; set; }
    public int Position { get; set; }

    public DictionaryCategory DictionaryCategory { get; set; }
    public DictionaryItem DictionaryItem { get; set; }
}
