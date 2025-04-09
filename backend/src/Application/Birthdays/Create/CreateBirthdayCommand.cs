using Application.Common.Messaging;

namespace Application.Birthdays.Create;

public sealed record CreateBirthdayCommand(
    string FullName,
    DateTime Date,
    string? Note) : ICommand<Guid>;
