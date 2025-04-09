namespace Domain.Birthdays;

public interface IBirthdayRepository
{
    Task InsertAsync(Birthday birthday, CancellationToken cancellationToken = default);
}
