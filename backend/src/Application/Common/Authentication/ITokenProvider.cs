using Application.Common.Models;
using Domain.Users;

namespace Application.Common.Authentication;

public interface ITokenProvider
{
    TokenInfo GenerateAccessToken(User user);

    TokenInfo GenerateRefreshToken();
}
