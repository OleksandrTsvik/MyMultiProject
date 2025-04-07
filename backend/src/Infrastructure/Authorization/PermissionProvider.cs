using Application.Common.Authorization;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Authorization;

public sealed class PermissionProvider : IPermissionProvider
{
    private readonly ApplicationDbContext _dbContext;

    public PermissionProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HashSet<UserPermissionType>> GetPermissionsAsync(Guid userId)
    {
        List<UserRole>? userRoles = await _dbContext.Users
            .Include(user => user.Roles)
                .ThenInclude(userRole => userRole.Permissions)
            .Where(user =>
                user.Id == userId &&
                user.DeletedOnUtc == null &&
                user.DeletedBy == null)
            .Select(user => user.Roles)
            .FirstOrDefaultAsync();

        if (userRoles is null)
        {
            return [];
        }

        return userRoles
            .SelectMany(userRole => userRole.Permissions)
            .Select(userPermission => userPermission.Name)
            .ToHashSet();
    }
}
