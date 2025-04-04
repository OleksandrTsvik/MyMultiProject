using Domain.Common;

namespace Domain.Users;

public sealed class UserRole : Entity
{
    public string Name { get; set; } = string.Empty;

    public List<User> Users { get; set; } = [];
    public List<UserPermission> Permissions { get; set; } = [];
}
