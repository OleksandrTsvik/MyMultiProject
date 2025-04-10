namespace Api.Contracts;

public record ErrorResponse<TDetails>(
    int StatusCode,
    string Code,
    string Message,
    TDetails? Details = default);

public sealed record ErrorResponse : ErrorResponse<object>
{
    public ErrorResponse(int StatusCode, string Code, string Message, object? Details = null)
        : base(StatusCode, Code, Message, Details)
    {
    }
}
