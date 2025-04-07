namespace Application.UnitTests.Abstractions;

public abstract class BaseValidatorTest
{
    public static List<string> GetValidationFailurePropertyNames(ValidationResult validationResult) =>
        validationResult.Errors
            .Select(static validationFailure => validationFailure.PropertyName)
            .ToList();
}
