using Application.Common.Messaging;

namespace Application.Birthdays.Delete;

public sealed record DeleteBirthdayCommand(Guid BirthdayId) : ICommand;
