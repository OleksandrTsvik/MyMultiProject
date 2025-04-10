namespace Application.Birthdays.Get;

public sealed record BirthdayResponse(
    Guid Id,
    string FullName,
    DateTime Date,
    string? Note,
    int Age,
    int DaysUntilBirthday);
