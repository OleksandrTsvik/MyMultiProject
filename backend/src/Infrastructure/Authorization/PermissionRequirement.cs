using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public string[] Permissions { get; }

    public PermissionRequirement(string[] permissions)
    {
        Permissions = permissions;
    }
}
