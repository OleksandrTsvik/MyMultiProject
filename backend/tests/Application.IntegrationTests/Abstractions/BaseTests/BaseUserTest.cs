using Application.Common.Authentication;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Abstractions.BaseTests;

public abstract class BaseUserTest : BaseIntegrationTest
{
    private readonly IPasswordHasher _passwordHasher;

    protected BaseUserTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _passwordHasher = Scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    }

    protected async Task<User> CreateUserAsync(
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

        DbContext.Users.Add(user);

        await DbContext.SaveChangesAsync();

        return user;
    }

    protected async Task<List<UserRole>> CreateUserRolesAsync()
    {
        List<UserPermission> permissions = await DbContext.UserPermissions.ToListAsync();

        var managerPermissions = new List<UserPermissionType>
        {
            UserPermissionType.FullAccess
        };

        var testerPermissions = new List<UserPermissionType>
        {
            UserPermissionType.FullAccess
        };

        var roles = new List<UserRole>
        {
            new()
            {
                Name = "Manager",
                Permissions = permissions
                    .Where(userPermission => managerPermissions.Contains(userPermission.Name))
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
}
