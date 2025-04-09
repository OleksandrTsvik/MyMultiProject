using SharedKernel;

namespace Domain.Birthdays;

public static class BirthdayErrors
{
    public static Error NotFoundById(Guid birthdayId) => Error.NotFound(
        "Users.NotFoundById",
        $"The birthday with the Id = '{birthdayId}' was not found.", birthdayId);
}
