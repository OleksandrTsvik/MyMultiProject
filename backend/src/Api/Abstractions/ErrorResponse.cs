namespace Api.Abstractions;

public sealed record ErrorResponse(
    int StatusCode,
    string Code,
    string Message,
    object? Details = null);
