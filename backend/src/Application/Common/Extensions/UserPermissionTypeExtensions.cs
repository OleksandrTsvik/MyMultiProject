using Domain.Users;

namespace Application.Common.Extensions;

public static class UserPermissionTypeExtensions
{
    public static bool ContainsPermission(
        this IEnumerable<UserPermissionType> userPermissionTypes,
        params UserPermissionType[] permissions)
    {
        return userPermissionTypes.Contains(UserPermissionType.FullAccess) ||
            permissions.Any(permission => userPermissionTypes.Contains(permission));
    }

    public static bool ContainsPermission(
        this IEnumerable<UserPermissionType> userPermissionTypes,
        params string[] permissions)
    {
        return userPermissionTypes.Contains(UserPermissionType.FullAccess) ||
            permissions.Any(permission =>
                Enum.TryParse(permission, out UserPermissionType parsed) &&
                userPermissionTypes.Contains(parsed));
    }
}
