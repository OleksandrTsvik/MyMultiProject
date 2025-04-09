using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFoundByEmail(string email) => Error.NotFound(
        "Users.NotFoundByEmail",
        $"The user with the Email = '{email}' was not found.", email);

    public static Error InvalidCredentials() => Error.Forbidden(
        "Users.InvalidCredentials",
        "Invalid email or password.");

    public static Error UserDeleted() => Error.Forbidden(
        "Users.UserDeleted",
        "User has been deleted.");

    public static Error EmailNotVerified(string email) => Error.Forbidden(
        "Users.EmailNotVerified",
        $"Email '{email}' is not verified.", email);

    public static Error InvalidPasswordHash() => Error.Forbidden(
        "Users.InvalidPasswordHash",
        "Invalid user password hash.");
}
