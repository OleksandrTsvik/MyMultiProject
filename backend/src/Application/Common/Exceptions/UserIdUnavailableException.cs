namespace Application.Common.Exceptions;

public sealed class UserIdUnavailableException : Exception
{
    public UserIdUnavailableException()
        : base("User id is unavailable.")
    {
    }
}
