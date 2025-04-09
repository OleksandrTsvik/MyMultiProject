using Application.Birthdays.Update;

namespace Application.UnitTests.Birthdays;

public class UpdateBirthdayCommandValidatorTests : BaseValidatorTest
{
    private readonly UpdateBirthdayCommandValidator _validator;

    public UpdateBirthdayCommandValidatorTests()
    {
        _validator = new UpdateBirthdayCommandValidator();
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
        var birthdayId = Guid.NewGuid();
        DateTime date = DateTime.UtcNow;

        var command = new UpdateBirthdayCommand(birthdayId, fullName!, date, note);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.NotEmpty(validationResult.Errors);

        List<string> failurePropertyNames = GetValidationFailurePropertyNames(validationResult);

        Assert.Contains(nameof(UpdateBirthdayCommand.FullName), failurePropertyNames);
    }

    [Theory]
    [InlineData("Ol", null)]
    [InlineData("Oleksandr Tsvik", "Test note.")]
    public async Task ValidateAsync_Should_Pass_WhenValidationSucceeds(
        string fullName,
        string? note)
    {
        // Arrange
        var birthdayId = Guid.NewGuid();
        DateTime date = DateTime.UtcNow;

        var command = new UpdateBirthdayCommand(birthdayId, fullName, date, note);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(validationResult.IsValid);
        Assert.Empty(validationResult.Errors);
    }
}
