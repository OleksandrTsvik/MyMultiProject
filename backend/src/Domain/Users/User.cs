using Domain.Common;

namespace Domain.Users;

public sealed class User : Entity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailVerified { get; set; } = false;
    public string? PasswordHash { get; set; }

    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedOnUtc { get; set; }
    public Guid? DeletedBy { get; set; }

    public List<UserRole> Roles { get; set; } = [];

    public bool IsDeleted => DeletedOnUtc is not null || DeletedBy is not null;

    public List<UserPermissionType> Permissions => Roles
        .SelectMany(userRole => userRole.Permissions)
        .Select(userPermission => userPermission.Name)
        .Distinct()
        .ToList();
}
