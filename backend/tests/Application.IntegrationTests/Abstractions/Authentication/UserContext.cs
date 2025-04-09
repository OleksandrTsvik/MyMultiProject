using Application.Common.Authentication;
using Domain.Users;
using Persistence;

namespace Application.IntegrationTests.Abstractions.Authentication;

public sealed class UserContext : IUserContext
{
    private static readonly Guid AuthorizedUserId = Guid.NewGuid();
    private readonly ApplicationDbContext _dbContext;

    public UserContext(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Guid UserId => GetUserId();

    public bool IsAuthenticated => true;

    private Guid GetUserId()
    {
        Guid userId = _dbContext.Users
            .Where(user => user.Id == AuthorizedUserId)
            .Select(user => user.Id)
            .FirstOrDefault();

        if (userId != Guid.Empty)
        {
            return userId;
        }

        User user = CreateAuthorizedUser();

        return user.Id;
    }

    private User CreateAuthorizedUser()
    {
        string uniquePostfix = AuthorizedUserId.ToString().Substring(19);

        var user = new User
        {
            Id = AuthorizedUserId,
            UserName = $"AuthorizedUser-{uniquePostfix}",
            Email = $"authorized.user.${uniquePostfix}@mail.com",
            EmailVerified = true,
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return user;
    }
}
