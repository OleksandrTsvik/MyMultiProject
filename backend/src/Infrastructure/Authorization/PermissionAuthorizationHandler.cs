using System.Security.Claims;
using Application.Common.Authorization;
using Application.Common.Extensions;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authorization;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        string? userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            return;
        }

        await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
        IPermissionProvider permissionProvider = scope.ServiceProvider.GetRequiredService<IPermissionProvider>();

        HashSet<UserPermissionType> userPermissionTypes = await permissionProvider.GetPermissionsAsync(parsedUserId);

        if (userPermissionTypes.ContainsPermission(requirement.Permissions))
        {
            context.Succeed(requirement);
        }
    }
}
