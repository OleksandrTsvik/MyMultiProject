namespace Api.Contracts.Birthdays;

public sealed record CreateBirthdayRequest(
    string FullName,
    DateTime Date,
    string? Note);
