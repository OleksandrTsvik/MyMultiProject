using Application.Users.Get;
using Domain.Users;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.IntegrationTests.Users;

public class GetUsersTests : BaseUserTest, IAsyncLifetime
{
    public static readonly string RequestUri = "api/users";

    public GetUsersTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync()
    {
        await DbContext.Users.ExecuteDeleteAsync();
        await DbContext.UserRoles.ExecuteDeleteAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_Should_ReturnOk_WhenSetPageSize()
    {
        // Arrange
        int pageSize = 2;

        await CreateManyUsersAsync(
            "test-01@mail.com",
            "test-02@mail.com",
            "test-03@mail.com");

        string requestUri = GenerateQueryString(pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(1, users.CurrentPage);
        Assert.Equal(2, users.TotalPages);
        Assert.Equal(4, users.TotalItems);
        Assert.False(users.HasPreviousPage);
        Assert.True(users.HasNextPage);

        Assert.Equal(pageSize, users.Items.Count);
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenSetPageNumber()
    {
        // Arrange
        int pageNumber = 2;
        int pageSize = 2;

        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");
        User user = await CreateUserAsync("test-03@mail.com");

        string requestUri = GenerateQueryString(pageNumber: pageNumber, pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(pageNumber, users.CurrentPage);
        Assert.Equal(2, users.TotalPages);
        Assert.Equal(4, users.TotalItems);
        Assert.True(users.HasPreviousPage);
        Assert.False(users.HasNextPage);

        Assert.NotEmpty(users.Items);
        AssertEqualUserResponse(user, users.Items.First());
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenFilterByUserName()
    {
        // Arrange
        string? userName = "test-03";
        int pageNumber = 1;
        int pageSize = 5;

        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");

        List<UserRole> roles = await CreateUserRolesAsync();
        User user = await CreateUserAsync("test-03@mail.com", null, roles);

        string requestUri = GenerateQueryString(
            userName: userName,
            pageNumber: pageNumber,
            pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(pageNumber, users.CurrentPage);
        Assert.Equal(1, users.TotalPages);
        Assert.Equal(1, users.TotalItems);
        Assert.False(users.HasPreviousPage);
        Assert.False(users.HasNextPage);

        Assert.Single(users.Items);
        AssertEqualUserResponse(user, users.Items.First());
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenFilterByEmail()
    {
        // Arrange
        string email = "test-03@mail.com";
        int pageNumber = 1;
        int pageSize = 5;

        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");

        List<UserRole> roles = await CreateUserRolesAsync();
        User user = await CreateUserAsync(email, null, roles);

        string requestUri = GenerateQueryString(
            email: email,
            pageNumber: pageNumber,
            pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(pageNumber, users.CurrentPage);
        Assert.Equal(1, users.TotalPages);
        Assert.Equal(1, users.TotalItems);
        Assert.False(users.HasPreviousPage);
        Assert.False(users.HasNextPage);

        Assert.Single(users.Items);
        AssertEqualUserResponse(user, users.Items.First());
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenFilterByUserNameAndEmail()
    {
        // Arrange
        string? userName = "test-05";
        string? email = "test-05@mail.com";
        int pageNumber = 1;
        int pageSize = 5;

        await CreateManyUsersAsync(
            "test-04@mail.com",
            "test-05@gmail.com",
            "user-test-05@mail.com",
            "test-055@mail.com");

        User user = await CreateUserAsync(email);

        string requestUri = GenerateQueryString(
            userName: userName,
            email: email,
            pageNumber: pageNumber,
            pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(pageNumber, users.CurrentPage);
        Assert.Equal(1, users.TotalPages);
        Assert.Equal(2, users.TotalItems);
        Assert.False(users.HasPreviousPage);
        Assert.False(users.HasNextPage);

        Assert.Equal(2, users.Items.Count);
        AssertEqualUserResponse(user, users.Items.First(userResponse => userResponse.Id == user.Id));
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenQueryStringIsEmpty()
    {
        // Arrange
        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");

        string requestUri = GenerateQueryString();

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(1, users.CurrentPage);
        Assert.Equal(1, users.TotalPages);
        Assert.Equal(3, users.TotalItems);
        Assert.False(users.HasPreviousPage);
        Assert.False(users.HasNextPage);
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenQueryStringIsValid()
    {
        // Arrange
        int pageNumber = 2;
        int pageSize = 5;

        await CreateManyUsersAsync(
            "01-test@mail.com",
            "02-test@mail.com",
            "03-test@mail.com",
            "04-test@mail.com",
            "05-test@mail.com",
            "06-test@mail.com",
            "07-test@mail.com",
            "oleksandr.zwick@gmail.com");

        string requestUri = GenerateQueryString(
            userName: "test",
            email: "test@mail.com",
            pageNumber: pageNumber,
            pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<UserResponse>? users = await response.GetContentAsync<PagedList<UserResponse>>();

        Assert.NotNull(users);
        Assert.Equal(pageNumber, users.CurrentPage);
        Assert.Equal(2, users.TotalPages);
        Assert.Equal(pageSize, users.PageSize);
        Assert.Equal(7, users.TotalItems);
        Assert.True(users.HasPreviousPage);
        Assert.False(users.HasNextPage);
    }

    private static string GenerateQueryString(
        string? userName = null,
        string? email = null,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = new Dictionary<string, string?>()
        {
            ["u"] = userName,
            ["e"] = email,
            ["p"] = pageNumber?.ToString(),
            ["ps"] = pageSize?.ToString(),
        };

        return QueryHelpers.AddQueryString(RequestUri, query);
    }

    private static void AssertEqualUserResponse(User user, UserResponse userResponse)
    {
        Assert.Equal(user.Id, userResponse.Id);
        Assert.Equal(user.UserName, userResponse.UserName);
        Assert.Equal(user.Email, userResponse.Email);
        Assert.Equal(user.EmailVerified, userResponse.EmailVerified);
        Assert.Equal(user.Roles.Select(role => role.Name).Order(), userResponse.Roles);
    }
}
