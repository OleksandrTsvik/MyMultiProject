using SharedKernel;

namespace Domain.Birthdays;

public static class BirthdayErrors
{
    public static Error NotFound() => Error.NotFound(
        "Birthdays.NotFound",
        "Birthday not found.");
}
