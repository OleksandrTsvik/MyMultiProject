using Application.Common.Authentication;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.Options;

namespace Persistence.Abstractions;

public sealed class ApplicationDbInitializer
{
    private readonly ILogger<ApplicationDbInitializer> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly ApplicationDbOptions _applicationDbOptions;
    private readonly SeedOptions _seedOptions;
    private readonly IPasswordHasher _passwordHasher;

    public ApplicationDbInitializer(
        ILogger<ApplicationDbInitializer> logger,
        ApplicationDbContext dbContext,
        IOptions<ApplicationDbOptions> applicationDbOptions,
        IOptions<SeedOptions> seedOptions,
        IPasswordHasher passwordHasher)
    {
        _logger = logger;
        _dbContext = dbContext;
        _seedOptions = seedOptions.Value;
        _applicationDbOptions = applicationDbOptions.Value;
        _passwordHasher = passwordHasher;
    }

    public async Task Execute(CancellationToken stoppingToken = default)
    {
        try
        {
            _logger.LogInformation("Starting database initialization.");

            ApplyMigrations();
            await SeedInitialDataAsync(stoppingToken);

            _logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }

    private void ApplyMigrations()
    {
        if (!_applicationDbOptions.ApplyMigrations)
        {
            _logger.LogInformation("ApplyMigrations is disabled in configuration. Skipping migrations.");
            return;
        }

        _logger.LogInformation("Applying database migrations.");
        _dbContext.Database.Migrate();
        _logger.LogInformation("Database migrations applied successfully.");
    }

    private async Task SeedInitialDataAsync(CancellationToken cancellationToken)
    {
        await SeedUserPermissionsAsync(cancellationToken);
        await SeedUserRolesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
    }

    private async Task SeedUserPermissionsAsync(CancellationToken cancellationToken)
    {
        UserPermissionType[] allUserPermissions = Enum.GetValues<UserPermissionType>();
        List<UserPermission> existingUserPermissions = await _dbContext.UserPermissions.ToListAsync(cancellationToken);

        var userPermissionsToCreate = new List<UserPermission>();
        var userPermissionsToDelete = new List<UserPermission>();

        foreach (UserPermission userPermission in existingUserPermissions)
        {
            if (!allUserPermissions.Contains(userPermission.Name))
            {
                userPermissionsToDelete.Add(userPermission);
            }
        }

        foreach (UserPermissionType userPermissionType in allUserPermissions)
        {
            if (!existingUserPermissions.Any(userPermission => userPermission.Name == userPermissionType))
            {
                userPermissionsToCreate.Add(new UserPermission { Name = userPermissionType });
            }
        }

        _dbContext.UserPermissions.RemoveRange(userPermissionsToDelete);
        _dbContext.UserPermissions.AddRange(userPermissionsToCreate);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedUserRolesAsync(CancellationToken cancellationToken)
    {
        bool hasAnyUserRoles = await _dbContext.UserRoles.AnyAsync(cancellationToken);

        if (hasAnyUserRoles)
        {
            return;
        }

        List<UserPermission> userPermissions = await _dbContext.UserPermissions.ToListAsync(cancellationToken);

        UserPermissionType[] adminPermissions = [UserPermissionType.FullAccess];

        UserRole[] userRoles =
        [
            new UserRole
            {
                Name = "Admin",
                Permissions = userPermissions
                    .Where(userPermission => adminPermissions.Contains(userPermission.Name))
                    .ToList()
            }
        ];

        _dbContext.UserRoles.AddRange(userRoles);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        bool hasAnyUsers = await _dbContext.Users.AnyAsync(cancellationToken);

        if (hasAnyUsers || _seedOptions.Users is null || _seedOptions.Users.Length == 0)
        {
            return;
        }

        var seedUserRoles = _seedOptions.Users
            .SelectMany(userSeed => userSeed?.Roles ?? [])
            .ToHashSet();

        List<UserRole> userRoles = await _dbContext.UserRoles
            .Where(userRole => seedUserRoles.Contains(userRole.Name))
            .ToListAsync(cancellationToken);

        var users = _seedOptions.Users
            .Select(user => new User
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailVerified = true,
                PasswordHash = _passwordHasher.Hash(user.Password),
                Roles = user.Roles is null
                    ? []
                    : userRoles
                        .Where(userRole => user.Roles.Contains(userRole.Name))
                        .ToList()
            })
            .ToList();

        _dbContext.AddRange(users);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
