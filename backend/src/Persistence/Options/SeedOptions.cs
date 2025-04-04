namespace Persistence.Options;

public sealed class SeedOptions
{
    public static readonly string ConfigurationSectionName = "Seed";

    public required UserSeed[] Users { get; init; }
}

public sealed class UserSeed
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string[]? Roles { get; init; }
}
