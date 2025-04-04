using System.Security.Claims;
using Application.Common.Exceptions;

namespace Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new UserIdUnavailableException();
    }
}
