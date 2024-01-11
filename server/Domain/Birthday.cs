namespace Domain;

public class Birthday
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime Date { get; set; }
#nullable enable
    public string? Note { get; set; }
#nullable disable

    public AppUser AppUser { get; set; }
}
