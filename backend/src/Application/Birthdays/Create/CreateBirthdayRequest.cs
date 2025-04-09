namespace Application.Birthdays.Create;

public sealed record CreateBirthdayRequest(
    string FullName,
    DateTime Date,
    string? Note);
