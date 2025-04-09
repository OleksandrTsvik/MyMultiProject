namespace Domain.Birthdays;

public interface IBirthdayRepository
{
    Task<Birthday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task InsertAsync(Birthday birthday, CancellationToken cancellationToken = default);

    Task UpdateAsync(Birthday birthday, CancellationToken cancellationToken = default);
}
