namespace Persistence.Options;

public sealed class ApplicationDbOptions
{
    public static readonly string ConfigurationSectionName = "ApplicationDb";

    public required string ConnectionString { get; init; }
    public required bool ApplyMigrations { get; init; }
}
