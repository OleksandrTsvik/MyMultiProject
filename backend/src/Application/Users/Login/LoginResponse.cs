using Domain.Users;

namespace Application.Users.Login;

public sealed record LoginResponse(
    string UserName,
    string Email,
    List<UserPermissionType> Permissions,
    string AccessToken,
    string RefreshToken);
