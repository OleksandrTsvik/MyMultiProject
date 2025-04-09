using Application.Birthdays.Update;
using Domain.Birthdays;

namespace Application.UnitTests.Birthdays;

public class UpdateBirthdayCommandHandlerTests
{
    private readonly Mock<IBirthdayRepository> _birthdayRepositoryMock;
    private readonly UpdateBirthdayCommandHandler _handler;

    public UpdateBirthdayCommandHandlerTests()
    {
        _birthdayRepositoryMock = new();

        _handler = new UpdateBirthdayCommandHandler(_birthdayRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenBirthdayNotFoundById()
    {
        // Arrange
        Birthday? birthday = null;

        var command = new UpdateBirthdayCommand(Guid.NewGuid(), "Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAsync(
                command.BirthdayId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(BirthdayErrors.NotFoundById(command.BirthdayId), result.Error);
    }

    [Fact]
    public async Task Handle_Should_NotCallUpdateAsync_WhenBirthdayNotFoundById()
    {
        // Arrange
        Birthday? birthday = null;

        var command = new UpdateBirthdayCommand(Guid.NewGuid(), "Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAsync(
                command.BirthdayId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _birthdayRepositoryMock.Verify(
            birthdayRepository => birthdayRepository.UpdateAsync(
                It.IsAny<Birthday>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData("Oleksandr", null)]
    [InlineData("Oleksandr Tsvik", "Test note.")]
    [InlineData("Tsvik Oleksandr", "...")]
    public async Task Handle_Should_ReturnSuccessResult_WhenUpdateSucceeds(string fullName, string? note)
    {
        // Arrange
        var birthday = new Birthday
        {
            Id = Guid.NewGuid(),
            FullName = "Oleksandr Tsvik",
            Date = DateTime.UtcNow.AddMinutes(-10),
            Note = "Test note.",
        };

        var command = new UpdateBirthdayCommand(birthday.Id, fullName, DateTime.UtcNow, note);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAsync(
                command.BirthdayId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("Oleksandr", null)]
    [InlineData("Oleksandr Tsvik", "Test note.")]
    [InlineData("Tsvik Oleksandr", "...")]
    public async Task Handle_Should_CallRepository_WhenUpdateSucceeds(string fullName, string? note)
    {
        // Arrange
        var birthday = new Birthday
        {
            Id = Guid.NewGuid(),
            FullName = "Oleksandr Tsvik",
            Date = DateTime.UtcNow.AddMinutes(-10),
            Note = "Test note.",
        };

        var command = new UpdateBirthdayCommand(birthday.Id, fullName, DateTime.UtcNow, note);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAsync(
                command.BirthdayId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        _birthdayRepositoryMock.Verify(
            birthdayRepository => birthdayRepository.GetByIdAsync(
                command.BirthdayId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _birthdayRepositoryMock.Verify(
            birthdayRepository => birthdayRepository.UpdateAsync(
                It.Is<Birthday>(birthday =>
                    birthday.Id == command.BirthdayId &&
                    birthday.UserId == birthday.UserId &&
                    birthday.FullName == command.FullName &&
                    birthday.Date == command.Date &&
                    birthday.Note == command.Note),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
