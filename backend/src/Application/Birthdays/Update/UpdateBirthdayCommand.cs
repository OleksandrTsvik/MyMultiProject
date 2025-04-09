using Application.Common.Messaging;

namespace Application.Birthdays.Update;

public sealed record UpdateBirthdayCommand(
    Guid BirthdayId,
    string FullName,
    DateTime Date,
    string? Note) : ICommand;
