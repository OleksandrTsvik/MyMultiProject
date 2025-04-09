using Application.Common.Authentication;
using Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Api.FunctionalTests.Abstractions.BaseTests;

public abstract class BaseUserTest : BaseFunctionalTest
{
    private readonly IPasswordHasher _passwordHasher;

    protected BaseUserTest(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
        _passwordHasher = Scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    }

    protected async Task<User> CreateUserAsync(
        string email,
        string? password = null,
        List<UserRole>? roles = null)
    {
        User user = CreateUserInstance(email, password, roles);

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        return user;
    }

    protected async Task<List<User>> CreateManyUsersAsync(string email, params string[] emails)
    {
        var users = new List<User>
        {
            CreateUserInstance(email)
        };

        users.AddRange(emails.Select(email => CreateUserInstance(email)));

        DbContext.Users.AddRange(users);
        await DbContext.SaveChangesAsync();

        return users;
    }

    protected async Task<List<UserRole>> CreateUserRolesAsync()
    {
        List<UserPermission> permissions = await DbContext.UserPermissions.ToListAsync();

        var adminPermissions = new List<UserPermissionType>
        {
            UserPermissionType.FullAccess,
        };

        var testerPermissions = new List<UserPermissionType>
        {
            UserPermissionType.ReadUser,
        };

        var roles = new List<UserRole>
        {
            new()
            {
                Name = "Admin",
                Permissions = permissions
                    .Where(userPermission => adminPermissions.Contains(userPermission.Name))
                    .ToList(),
            },
            new()
            {
                Name = "Tester",
                Permissions = permissions
                    .Where(userPermission => testerPermissions.Contains(userPermission.Name))
                    .ToList(),
            },
        };

        DbContext.UserRoles.AddRange(roles);
        await DbContext.SaveChangesAsync();

        return roles;
    }

    protected User CreateUserInstance(
        string email,
        string? password = null,
        List<UserRole>? roles = null)
    {
        string? passwordHash = string.IsNullOrWhiteSpace(password) ? null : _passwordHasher.Hash(password);

        var user = new User
        {
            UserName = email,
            Email = email,
            EmailVerified = true,
            PasswordHash = passwordHash,
            Roles = roles ?? [],
        };

        return user;
    }
}
