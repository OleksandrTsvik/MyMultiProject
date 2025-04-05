using Application.Common.Authentication;
using Application.Common.Models;
using Application.Users.Login;
using Domain.Users;

namespace Application.UnitTests.Users;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenProvider> _tokenProviderMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;

    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _passwordHasherMock = new();
        _tokenProviderMock = new();
        _refreshTokenRepositoryMock = new();

        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _tokenProviderMock.Object,
            _refreshTokenRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenUserNotFoundByEmail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        User? user = null;

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Result<LoginResponse> result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(UserErrors.InvalidCredentials(), result.Error);
    }

    [Fact]
    public async Task Handle_Should_NotCallRefreshTokenRepository_WhenUserNotFoundByEmail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        User? user = null;

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _refreshTokenRepositoryMock.Verify(
            refreshTokenRepository => refreshTokenRepository.InsertAsync(
                It.IsAny<RefreshToken>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenUserIsDeleted()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { DeletedOnUtc = DateTime.UtcNow };

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Result<LoginResponse> result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(UserErrors.UserDeleted(), result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenUserPasswordHashIsEmpty()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { PasswordHash = string.Empty };

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Result<LoginResponse> result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(UserErrors.InvalidPasswordHash(), result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { PasswordHash = "password_hash" };

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.Verify(command.Password, user.PasswordHash))
            .Returns(false);

        // Act
        Result<LoginResponse> result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(UserErrors.InvalidCredentials(), result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenLoginSucceeds()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { PasswordHash = "password_hash" };
        var accessTokenInfo = new TokenInfo("access_token", DateTime.UtcNow);
        var refreshTokenInfo = new TokenInfo("refresh_token", DateTime.UtcNow);

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.Verify(command.Password, user.PasswordHash))
            .Returns(true);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateAccessToken(user))
            .Returns(accessTokenInfo);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateRefreshToken())
            .Returns(refreshTokenInfo);

        // Act
        Result<LoginResponse> result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_Should_CallRefreshTokenRepository_WhenLoginSucceeds()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { PasswordHash = "password_hash" };
        var accessTokenInfo = new TokenInfo("access_token", DateTime.UtcNow);
        var refreshTokenInfo = new TokenInfo("refresh_token", DateTime.UtcNow);

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.Verify(command.Password, user.PasswordHash))
            .Returns(true);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateAccessToken(user))
            .Returns(accessTokenInfo);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateRefreshToken())
            .Returns(refreshTokenInfo);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _refreshTokenRepositoryMock.Verify(
            refreshTokenRepository => refreshTokenRepository.InsertAsync(
                It.IsAny<RefreshToken>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallTokenProvider_WhenLoginSucceeds()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { PasswordHash = "password_hash" };
        var accessTokenInfo = new TokenInfo("access_token", DateTime.UtcNow);
        var refreshTokenInfo = new TokenInfo("refresh_token", DateTime.UtcNow);

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.Verify(command.Password, user.PasswordHash))
            .Returns(true);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateAccessToken(user))
            .Returns(accessTokenInfo);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateRefreshToken())
            .Returns(refreshTokenInfo);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _tokenProviderMock.Verify(
            tokenProvider => tokenProvider.GenerateAccessToken(user),
            Times.Once);

        _tokenProviderMock.Verify(
            tokenProvider => tokenProvider.GenerateRefreshToken(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_GenerateTokens_WhenLoginSucceeds()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "password");
        var user = new User { PasswordHash = "password_hash" };
        var accessTokenInfo = new TokenInfo("access_token", DateTime.UtcNow);
        var refreshTokenInfo = new TokenInfo("refresh_token", DateTime.UtcNow);

        _userRepositoryMock.Setup(userRepository =>
            userRepository.GetByEmailWithRolesAndPermissionsAsync(
                command.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(passwordHasher => passwordHasher.Verify(command.Password, user.PasswordHash))
            .Returns(true);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateAccessToken(user))
            .Returns(accessTokenInfo);

        _tokenProviderMock
            .Setup(tokenProvider => tokenProvider.GenerateRefreshToken())
            .Returns(refreshTokenInfo);

        // Act
        Result<LoginResponse> result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Assert.NotNull(result.Value);
        Assert.Equal(accessTokenInfo.Token, result.Value.AccessToken);
        Assert.Equal(refreshTokenInfo.Token, result.Value.RefreshToken);
    }
}
