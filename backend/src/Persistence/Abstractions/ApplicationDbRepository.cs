using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Abstractions;

internal abstract class ApplicationDbRepository<TEntity>
    where TEntity : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected ApplicationDbRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Add(entity);

        return DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Update(entity);

        return DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().Remove(entity);

        return DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);

        return DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<TEntity>().AnyAsync(entity => entity.Id == id, cancellationToken);
    }
}
