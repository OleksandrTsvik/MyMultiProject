namespace Application.Birthdays.DTOs;

public class CreateDto
{
    public string FullName { get; set; }
    public DateTime Date { get; set; }
#nullable enable
    public string? Note { get; set; }
#nullable disable
}
