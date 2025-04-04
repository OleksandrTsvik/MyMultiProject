using Domain.Users;
using Persistence.Abstractions;

namespace Persistence.Repositories;

internal sealed class RefreshTokenRepository : ApplicationDbRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
