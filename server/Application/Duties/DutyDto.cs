namespace Application.Duties;

public class DutyDto
{
    public Guid Id { get; set; }
    public int Position { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DateCreation { get; set; }
#nullable enable
    public DateTime? DateCompletion { get; set; }
    public string? BackgroundColor { get; set; }
    public string? FontColor { get; set; }
#nullable disable
}