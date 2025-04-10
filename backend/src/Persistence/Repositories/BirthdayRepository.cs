using Domain.Birthdays;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstractions;

namespace Persistence.Repositories;

internal sealed class BirthdayRepository : ApplicationDbRepository<Birthday>, IBirthdayRepository
{
    public BirthdayRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public Task<Birthday?> GetByIdAndUserIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return DbContext.Birthdays
            .AsNoTracking()
            .FirstOrDefaultAsync(
                birthday =>
                    birthday.Id == id &&
                    birthday.UserId == userId,
                cancellationToken);
    }

    public Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return DbContext.Birthdays
            .Where(
                birthday =>
                    birthday.Id == id &&
                    birthday.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
