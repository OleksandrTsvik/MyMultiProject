using Domain.Common;
using Domain.Users;

namespace Domain.Birthdays;

public sealed class Birthday : Entity
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Note { get; set; }

    public User? User { get; set; }
}
