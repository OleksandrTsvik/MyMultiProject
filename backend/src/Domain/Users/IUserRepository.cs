namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByEmailWithRolesAndPermissionsAsync(string email, CancellationToken cancellationToken = default);
}
