using Domain.Users;

namespace Application.Common.Authorization;

public interface IPermissionProvider
{
    Task<HashSet<UserPermissionType>> GetPermissionsAsync(Guid userId);
}
