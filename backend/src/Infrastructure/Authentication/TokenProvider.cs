using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Authentication;
using Application.Common.Models;
using Domain.Users;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public TokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public TokenInfo GenerateAccessToken(User user)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        DateTime expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = signingCredentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var jsonWebTokenHandler = new JsonWebTokenHandler();

        string token = jsonWebTokenHandler.CreateToken(tokenDescriptor);

        return new TokenInfo(token, expires);
    }

    public TokenInfo GenerateRefreshToken()
    {
        DateTime expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays);
        string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        return new TokenInfo(token, expires);
    }
}
