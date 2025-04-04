namespace Api.Options.Models;

public sealed class CorsOptions
{
    public static readonly string ConfigurationSectionName = "Cors";

    public required string[] Origins { get; init; }
}
