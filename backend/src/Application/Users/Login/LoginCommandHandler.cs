using Application.Common.Authentication;
using Application.Common.Messaging;
using Application.Common.Models;
using Domain.Users;
using SharedKernel;

namespace Application.Users.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailWithRolesAndPermissionsAsync(
            request.Email, cancellationToken);

        if (user is null)
        {
            return UserErrors.InvalidCredentials();
        }

        if (user.IsDeleted)
        {
            return UserErrors.UserDeleted();
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            return UserErrors.InvalidPasswordHash();
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return UserErrors.InvalidCredentials();
        }

        TokenInfo accessTokenInfo = _tokenProvider.GenerateAccessToken(user);
        TokenInfo refreshTokenInfo = _tokenProvider.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenInfo.Token,
            ExpiresOnUtc = refreshTokenInfo.ExpiresOnUtc
        };

        await _refreshTokenRepository.InsertAsync(refreshToken, cancellationToken);

        return new LoginResponse(
            user.UserName,
            user.Email,
            user.Permissions,
            accessTokenInfo.Token,
            refreshToken.Token);
    }
}
