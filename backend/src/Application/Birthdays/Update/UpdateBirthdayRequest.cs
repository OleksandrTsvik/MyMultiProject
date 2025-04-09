namespace Application.Birthdays.Update;

public sealed record UpdateBirthdayRequest(
    string FullName,
    DateTime Date,
    string? Note);
