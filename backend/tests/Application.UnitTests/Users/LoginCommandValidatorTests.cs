using Application.Users.Login;

namespace Application.UnitTests.Users;

public class LoginCommandValidatorTests : BaseValidatorTest
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    public async Task ValidateAsync_Should_FailValidation_WhenEmailAndPasswordIsEmpty(
        string? email,
        string? password)
    {
        // Arrange
        var command = new LoginCommand(email!, password!);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.NotEmpty(validationResult.Errors);

        List<string> failurePropertyNames = GetValidationFailurePropertyNames(validationResult);

        Assert.Contains(nameof(LoginCommand.Email), failurePropertyNames);
        Assert.Contains(nameof(LoginCommand.Password), failurePropertyNames);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("test.mail")]
    [InlineData("test@@mail")]
    public async Task ValidateAsync_Should_FailValidation_WhenEmailIsInvalid(string? email)
    {
        // Arrange
        string password = "password";

        var command = new LoginCommand(email!, password);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.NotEmpty(validationResult.Errors);

        List<string> failurePropertyNames = GetValidationFailurePropertyNames(validationResult);

        Assert.Contains(nameof(LoginCommand.Email), failurePropertyNames);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task ValidateAsync_Should_FailValidation_WhenPasswordIsInvalid(string? password)
    {
        // Arrange
        string email = "oleksandr.zwick@gmail.com";

        var command = new LoginCommand(email, password!);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.NotEmpty(validationResult.Errors);
        Assert.Single(validationResult.Errors);

        List<string> failurePropertyNames = GetValidationFailurePropertyNames(validationResult);

        Assert.Contains(nameof(LoginCommand.Password), failurePropertyNames);
    }

    [Fact]
    public async Task ValidateAsync_Should_Pass_WhenValidationSucceeds()
    {
        // Arrange
        string email = "oleksandr.zwick@gmail.com";
        string password = "password";

        var command = new LoginCommand(email, password);

        // Act
        ValidationResult validationResult = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(validationResult.IsValid);
        Assert.Empty(validationResult.Errors);
    }
}
