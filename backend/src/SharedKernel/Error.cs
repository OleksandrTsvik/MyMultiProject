namespace SharedKernel;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided.");
    public static readonly Error NullResult = new("Error.NullResult", "Null result was provided.");

    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public object[] MessageArguments { get; }

    private Error(
        string code,
        string message,
        ErrorType type = ErrorType.Failure,
        object[]? messageArguments = null)
    {
        Code = code;
        Message = message;
        Type = type;
        MessageArguments = messageArguments ?? [];
    }

    public static Error Failure(string code, string message, params object[] messageArguments) =>
        new(code, message, ErrorType.Failure, messageArguments);

    public static Error NotFound(string code, string message, params object[] messageArguments) =>
        new(code, message, ErrorType.NotFound, messageArguments);

    public static Error Validation(string code, string message, params object[] messageArguments) =>
        new(code, message, ErrorType.Validation, messageArguments);

    public static Error Conflict(string code, string message, params object[] messageArguments) =>
        new(code, message, ErrorType.Conflict, messageArguments);

    public static Error Unauthorized(string code, string message, params object[] messageArguments) =>
        new(code, message, ErrorType.Unauthorized, messageArguments);

    public static Error Forbidden(string code, string message, params object[] messageArguments) =>
        new(code, message, ErrorType.Forbidden, messageArguments);
}
