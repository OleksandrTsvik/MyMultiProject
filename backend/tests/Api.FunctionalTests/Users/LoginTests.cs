using Application.Users.Login;
using Domain.Users;

namespace Api.FunctionalTests.Users;

public class LoginTests : BaseUserTest, IAsyncLifetime
{
    public static readonly string RequestUri = "api/users/login";

    public LoginTests(FunctionalTestWebAppFactory factory)
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
    public async Task Post_Should_ReturnBadRequest_WhenBodyIsEmpty()
    {
        // Arrange
        object request = new();

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Contains(nameof(LoginCommand.Email), errorResponse.Details.Select(error => error.Code));
        Assert.Contains(nameof(LoginCommand.Password), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Post_Should_ReturnBadRequest_WhenEmailIsMissing()
    {
        // Arrange
        var request = new LoginRequest("", "password");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Contains(nameof(LoginCommand.Email), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Post_Should_ReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new LoginRequest("test.gmail", "password");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Single(errorResponse.Details);
        Assert.Contains(nameof(LoginCommand.Email), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Post_Should_ReturnBadRequest_WhenPasswordIsMissing()
    {
        // Arrange
        var request = new LoginRequest("test@mail.com", "");

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Single(errorResponse.Details);
        Assert.Contains(nameof(LoginCommand.Password), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Post_Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        string email = "test@mail.com";
        string password = "password";

        List<UserRole> roles = await CreateUserRolesAsync();
        User user = await CreateUserAsync(email, password, roles);

        var request = new LoginRequest(email, password);

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        LoginResponse? loginResponse = await response.GetContentAsync<LoginResponse>();

        Assert.NotNull(loginResponse);
        Assert.Equal(user.Email, loginResponse.Email);
        Assert.Equal(user.Permissions.Order(), loginResponse.Permissions.Order());
        Assert.False(string.IsNullOrWhiteSpace(loginResponse.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(loginResponse.RefreshToken));
    }
}
