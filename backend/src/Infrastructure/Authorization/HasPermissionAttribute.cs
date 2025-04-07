using Domain.Users;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "PERMISSION:";

    public HasPermissionAttribute(params UserPermissionType[] permissions)
    {
        Policy = $"{PolicyPrefix}{string.Join(",", permissions)}";
    }
}
