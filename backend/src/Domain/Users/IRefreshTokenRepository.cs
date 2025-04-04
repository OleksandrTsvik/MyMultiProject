namespace Domain.Users;

public interface IRefreshTokenRepository
{
    Task InsertAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
