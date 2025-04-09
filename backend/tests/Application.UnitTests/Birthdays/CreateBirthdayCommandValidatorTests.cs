using Application.Birthdays.Create;

namespace Application.UnitTests.Birthdays;

public class CreateBirthdayCommandValidatorTests : BaseValidatorTest
{
    private readonly CreateBirthdayCommandValidator _validator;

    public CreateBirthdayCommandValidatorTests()
    {
        _validator = new CreateBirthdayCommandValidator();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("A", "Test note.")]
    [InlineData("A  ", "Test note.")]
    [InlineData(" A", "Test note.")]
    [InlineData("   ", "Test note.")]
    public async Task ValidateAsync_Should_FailValidation_WhenFullNameIsInvalid(
        string? fullName,
        string? note)
    {
        // Arrange
        DateTime date = DateTime.UtcNow;

        var command = new CreateBirthdayCommand(fullName!, date, note);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.NotEmpty(validationResult.Errors);

        List<string> failurePropertyNames = GetValidationFailurePropertyNames(validationResult);

        Assert.Contains(nameof(CreateBirthdayCommand.FullName), failurePropertyNames);
    }

    [Theory]
    [InlineData("Ol", null)]
    [InlineData("Oleksandr Tsvik", "Test note.")]
    public async Task ValidateAsync_Should_Pass_WhenValidationSucceeds(
        string fullName,
        string? note)
    {
        // Arrange
        DateTime date = DateTime.UtcNow;

        var command = new CreateBirthdayCommand(fullName, date, note);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(validationResult.IsValid);
        Assert.Empty(validationResult.Errors);
    }
}
