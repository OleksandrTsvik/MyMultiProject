using Domain.Common;

namespace Domain.Users;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOnUtc { get; set; }

    public User? User { get; set; }
}
