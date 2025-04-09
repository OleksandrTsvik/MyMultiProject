using Application.Users.Get;
using Domain.Users;

namespace Application.IntegrationTests.Users;

public class GetUsersQueryHandlerTests : BaseUserTest, IAsyncLifetime
{
    public GetUsersQueryHandlerTests(IntegrationTestWebAppFactory factory)
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
    public async Task Handle_Should_ReturnEmptyList_WhenUsersDoesNotExist()
    {
        // Arrange
        string? userName = null;
        string? email = null;
        int? pageNumber = null;
        int? pageSize = null;

        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        // Act
        Result<PagedList<UserResponse>> result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(0, result.Value.TotalPages);
        Assert.False(result.Value.HasPreviousPage);
        Assert.False(result.Value.HasNextPage);

        Assert.Empty(result.Value.Items);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedResult_WhenSetPageSize()
    {
        // Arrange
        string? userName = null;
        string? email = null;
        int pageNumber = 1;
        int pageSize = 2;

        await CreateManyUsersAsync(
            "test-01@mail.com",
            "test-02@mail.com",
            "test-03@mail.com");

        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        // Act
        Result<PagedList<UserResponse>> result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.TotalItems);
        Assert.False(result.Value.HasPreviousPage);
        Assert.True(result.Value.HasNextPage);

        Assert.Equal(pageSize, result.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedResult_WhenSetPageNumber()
    {
        // Arrange
        string? userName = null;
        string? email = null;
        int pageNumber = 2;
        int pageSize = 2;

        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");
        User user = await CreateUserAsync("test-03@mail.com");

        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        // Act
        Result<PagedList<UserResponse>> result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.TotalItems);
        Assert.True(result.Value.HasPreviousPage);
        Assert.False(result.Value.HasNextPage);

        Assert.Single(result.Value.Items);
        AssertEqualUserResponse(user, result.Value.Items.First());
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedResult_WhenFilterByUserName()
    {
        // Arrange
        string? userName = "test-03";
        string? email = null;
        int pageNumber = 1;
        int pageSize = 5;

        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");

        List<UserRole> roles = await CreateUserRolesAsync();
        User user = await CreateUserAsync("test-03@mail.com", null, roles);

        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        // Act
        Result<PagedList<UserResponse>> result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.TotalItems);
        Assert.False(result.Value.HasPreviousPage);
        Assert.False(result.Value.HasNextPage);

        Assert.Single(result.Value.Items);
        AssertEqualUserResponse(user, result.Value.Items.First());
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedResult_WhenFilterByEmail()
    {
        // Arrange
        string? userName = null;
        string? email = "test-03@mail.com";
        int pageNumber = 1;
        int pageSize = 5;

        await CreateManyUsersAsync("test-01@mail.com", "test-02@mail.com");

        List<UserRole> roles = await CreateUserRolesAsync();
        User user = await CreateUserAsync(email, null, roles);

        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        // Act
        Result<PagedList<UserResponse>> result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.TotalItems);
        Assert.False(result.Value.HasPreviousPage);
        Assert.False(result.Value.HasNextPage);

        Assert.Single(result.Value.Items);
        AssertEqualUserResponse(user, result.Value.Items.First());
    }

    [Fact]
    public async Task Handle_Should_ReturnPagedResult_WhenFilterByUserNameAndEmail()
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

        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        // Act
        Result<PagedList<UserResponse>> result = await Sender.Send(query);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.TotalItems);
        Assert.False(result.Value.HasPreviousPage);
        Assert.False(result.Value.HasNextPage);

        Assert.Equal(2, result.Value.Items.Count);
        AssertEqualUserResponse(user, result.Value.Items.First(userResponse => userResponse.Id == user.Id));
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
