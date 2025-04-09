using Application.Users.Get;
using Microsoft.AspNetCore.WebUtilities;

namespace Api.FunctionalTests.Users;

public class GetUsersTests : BaseUserTest, IAsyncLifetime
{
    public static readonly string RequestUri = "api/users";

    public GetUsersTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync()
    {
        await DbContext.Users.ExecuteDeleteAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

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
}
