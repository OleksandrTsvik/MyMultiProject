namespace Application.Users.Get;

public sealed record UserResponse(
    Guid Id,
    string UserName,
    string Email,
    bool EmailVerified,
    string[] Roles);
