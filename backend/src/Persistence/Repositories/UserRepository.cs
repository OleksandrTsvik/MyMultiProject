using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstractions;

namespace Persistence.Repositories;

internal sealed class UserRepository : ApplicationDbRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public Task<User?> GetByEmailWithRolesAndPermissionsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return DbContext.Users
            .AsNoTracking()
            .Include(user => user.Roles)
                .ThenInclude(userRole => userRole.Permissions)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }
}
