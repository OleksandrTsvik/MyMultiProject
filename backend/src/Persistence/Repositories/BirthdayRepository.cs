using Domain.Birthdays;
using Persistence.Abstractions;

namespace Persistence.Repositories;

internal sealed class BirthdayRepository : ApplicationDbRepository<Birthday>, IBirthdayRepository
{
    public BirthdayRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
