using Application.Birthdays.Update;
using Application.Common.Authentication;
using Domain.Birthdays;

namespace Application.UnitTests.Birthdays;

public class UpdateBirthdayCommandHandlerTests
{
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IBirthdayRepository> _birthdayRepositoryMock;

    private readonly UpdateBirthdayCommandHandler _handler;

    public UpdateBirthdayCommandHandlerTests()
    {
        _userContextMock = new();
        _birthdayRepositoryMock = new();

        _handler = new UpdateBirthdayCommandHandler(
            _userContextMock.Object,
            _birthdayRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenBirthdayNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Birthday? birthday = null;

        var command = new UpdateBirthdayCommand(Guid.NewGuid(), "Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(userId);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAndUserIdAsync(
                command.BirthdayId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(BirthdayErrors.NotFound(), result.Error);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenTryingUpdateAnotherUsersBirthday()
    {
        // Arrange
        var otherUserBirthday = new Birthday
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FullName = "Oleksandr Tsvik",
            Date = DateTime.UtcNow.AddMinutes(-10),
            Note = "Test note.",
        };

        var userId = Guid.NewGuid();
        Birthday? birthday = null;

        var command = new UpdateBirthdayCommand(
            otherUserBirthday.Id,
            "Oleksandr Tsvik",
            DateTime.UtcNow,
            "Test note.");

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(otherUserBirthday.UserId);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAndUserIdAsync(
                command.BirthdayId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equivalent(BirthdayErrors.NotFound(), result.Error);
    }

    [Fact]
    public async Task Handle_Should_NotCallUpdateAsync_WhenBirthdayNotFoundById()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Birthday? birthday = null;

        var command = new UpdateBirthdayCommand(Guid.NewGuid(), "Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(userId);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAndUserIdAsync(
                command.BirthdayId,
                userId,
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
            UserId = Guid.NewGuid(),
            FullName = "Oleksandr Tsvik",
            Date = DateTime.UtcNow.AddMinutes(-10),
            Note = "Test note.",
        };

        var command = new UpdateBirthdayCommand(birthday.Id, fullName, DateTime.UtcNow, note);

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(birthday.UserId);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAndUserIdAsync(
                command.BirthdayId,
                birthday.UserId,
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
            UserId = Guid.NewGuid(),
            FullName = "Oleksandr Tsvik",
            Date = DateTime.UtcNow.AddMinutes(-10),
            Note = "Test note.",
        };

        var command = new UpdateBirthdayCommand(birthday.Id, fullName, DateTime.UtcNow, note);

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(birthday.UserId);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.GetByIdAndUserIdAsync(
                command.BirthdayId,
                birthday.UserId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(birthday);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        _birthdayRepositoryMock.Verify(
            birthdayRepository => birthdayRepository.GetByIdAndUserIdAsync(
                command.BirthdayId,
                birthday.UserId,
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
