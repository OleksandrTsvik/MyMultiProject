using Domain.Common;

namespace Domain.Users;

public sealed class UserPermission : Entity
{
    public UserPermissionType Name { get; set; }

    public List<UserRole> Roles { get; set; } = [];
}
