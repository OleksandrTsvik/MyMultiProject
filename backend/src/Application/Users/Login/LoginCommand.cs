using Application.Common.Messaging;

namespace Application.Users.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
