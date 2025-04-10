namespace Api.Contracts.Birthdays;

public sealed record UpdateBirthdayRequest(
    string FullName,
    DateTime Date,
    string? Note);
