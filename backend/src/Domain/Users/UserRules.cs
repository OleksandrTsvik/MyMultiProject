namespace Domain.Users;

public static class UserRules
{
    public const int MinUserNameLength = 2;
    public const int MaxUserNameLength = 32;

    public const int MaxEmailLength = 128;

    public const int MinPasswordLength = 6;
    public const int MaxPasswordLength = 32;
}
