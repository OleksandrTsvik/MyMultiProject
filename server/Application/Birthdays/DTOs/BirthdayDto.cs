namespace Application.Birthdays.DTOs;

public class BirthdayDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime Date { get; set; }
    public string Note { get; set; }
    public int Age { get; set; }
    public int DaysUntilBirthday { get; set; }
}
