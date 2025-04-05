using Application.Users.Login;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests.Users;

public class LoginCommandHandlerTests : BaseUserTest, IAsyncLifetime
{
    public LoginCommandHandlerTests(IntegrationTestWebAppFactory factory)
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
    public async Task Handle_Should_ReturnFailureResult_WhenUserNotFoundByEmail()
    {
        // Arrange
        var command = new LoginCommand("test@mail.com", "password");

        // Act
        Result<LoginResponse> result = await Sender.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenPasswordIsInvalid()
    {
        // Arrange
        string email = "test@mail.com";
        string password = "password";
        string invalidPassword = "invalid-password";

        User user = await CreateUserAsync(email, password);
        var command = new LoginCommand(user.Email, invalidPassword);

        // Act
        Result<LoginResponse> result = await Sender.Send(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenLoginSucceeds()
    {
        // Arrange
        string email = "test@mail.com";
        string password = "password";

        List<UserRole> roles = await CreateUserRolesAsync();
        User user = await CreateUserAsync(email, password, roles);

        var command = new LoginCommand(email, password);

        // Act
        Result<LoginResponse> result = await Sender.Send(command);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(user.UserName, result.Value.UserName);
        Assert.Equal(user.Email, result.Value.Email);
        Assert.Equal(user.Permissions, result.Value.Permissions);

        RefreshToken? refreshToken = await DbContext.RefreshTokens
            .FirstOrDefaultAsync(refreshToken => refreshToken.Token == result.Value.RefreshToken);

        Assert.NotNull(refreshToken);
    }
}
