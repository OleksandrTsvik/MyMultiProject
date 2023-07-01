namespace Application.Core;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Value { get; set; }
    public string Error { get; set; }
    public string ModelKey { get; set; }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value
        };
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = error
        };
    }

    public static Result<T> ValidationFailure(string modelKey, string error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = error,
            ModelKey = modelKey
        };
    }
}