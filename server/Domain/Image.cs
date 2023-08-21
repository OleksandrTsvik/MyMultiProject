namespace Domain;

public class Image
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime DateCreation { get; set; }

    public AppUser AppUser { get; set; }
}
