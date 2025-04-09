using System.Net.Http.Headers;
using Application.Common.Authentication;
using Application.Users.Login;
using Domain.Users;
using Persistence;

namespace Api.FunctionalTests.Abstractions.Services;

public sealed class AuthenticationService
{
    public static readonly string AuthenticationScheme = "Bearer";

    private static readonly string DefaultAuthorizedUserEmail = "test.authorized.user@mail.com";
    private static readonly string DefaultAuthorizedUserPassword = "password";
    private static readonly string DefaultAuthorizedUserRole = "TestAuthorizedUserRole";

    private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticationService(
        HttpClient httpClient,
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(
        List<UserPermissionType>? permissions = null)
    {
        string accessToken = await GetAccessTokenAsync(permissions);

        return new AuthenticationHeaderValue(AuthenticationScheme, accessToken);
    }

    public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(
        string email,
        string password,
        List<UserPermissionType> permissions)
    {
        string accessToken = await GetAccessTokenAsync(email, password, permissions);

        return new AuthenticationHeaderValue(AuthenticationScheme, accessToken);
    }

    public Task<string> GetAccessTokenAsync(List<UserPermissionType>? permissions = null)
    {
        return GetAccessTokenAsync(
            DefaultAuthorizedUserEmail,
            DefaultAuthorizedUserPassword,
            permissions is null ? [UserPermissionType.FullAccess] : permissions);
    }

    public async Task<string> GetAccessTokenAsync(
        string email,
        string password,
        List<UserPermissionType> permissions)
    {
        User user = await CreateOrUpdateAuthorizedUserAsync(email, password, permissions);

        var request = new LoginRequest(user.Email, password);

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users/login", request);
        LoginResponse? loginResponse = await response.GetContentAsync<LoginResponse>();

        ArgumentNullException.ThrowIfNull(loginResponse);

        return loginResponse.AccessToken;
    }

    private async Task<User> CreateOrUpdateAuthorizedUserAsync(
        string email,
        string password,
        List<UserPermissionType> permissions)
    {
        User? user = await GetUserAsync(email);

        user = user is null
            ? CreateAuthorizedUser(email, password)
            : UpdateAuthorizedUser(user, password);

        await UpdateAuthorizedUserRoleAsync(user, permissions);

        await _dbContext.SaveChangesAsync();

        return user;
    }

    private Task<User?> GetUserAsync(string email)
    {
        return _dbContext.Users
            .Include(user => user.Roles)
                .ThenInclude(userRole => userRole.Permissions)
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    private User UpdateAuthorizedUser(User user, string password)
    {
        user.PasswordHash = _passwordHasher.Hash(password);

        return user;
    }

    private User CreateAuthorizedUser(string email, string password)
    {
        string passwordHash = _passwordHasher.Hash(password);

        var user = new User
        {
            UserName = email,
            Email = email,
            EmailVerified = true,
            PasswordHash = passwordHash,
        };

        _dbContext.Users.Add(user);

        return user;
    }

    private async Task UpdateAuthorizedUserRoleAsync(User user, List<UserPermissionType> permissions)
    {
        List<UserPermissionType> userPermissions = user.Permissions;

        if (userPermissions.Count >= permissions.Count &&
            permissions.All(permission => userPermissions.Contains(permission)))
        {
            return;
        }

        UserRole? userRole = await _dbContext.UserRoles
            .Include(role => role.Permissions)
            .FirstOrDefaultAsync(role => role.Name == DefaultAuthorizedUserRole);

        if (userRole is null)
        {
            userRole = new UserRole
            {
                Name = DefaultAuthorizedUserRole,
            };

            _dbContext.UserRoles.Add(userRole);
        }

        userRole.Permissions = await _dbContext.UserPermissions
            .Where(userPermission => permissions.Contains(userPermission.Name))
            .ToListAsync();

        if (!user.Roles.Any(role => role.Name == userRole.Name))
        {
            user.Roles.Add(userRole);
        }
    }
}
