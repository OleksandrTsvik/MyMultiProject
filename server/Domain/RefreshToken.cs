namespace Domain;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime? Revoked { get; set; }

    public AppUser AppUser { get; set; }

    public RefreshToken()
    {
        Expires = DateTime.UtcNow.AddDays(7);
    }

    public bool IsExpired
    {
        get { return DateTime.UtcNow >= Expires; }
    }

    public bool IsActive
    {
        get { return Revoked == null && !IsExpired; }
    }
}