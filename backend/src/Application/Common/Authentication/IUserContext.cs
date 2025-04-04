namespace Application.Common.Authentication;

public interface IUserContext
{
    Guid UserId { get; }

    bool IsAuthenticated { get; }
}
