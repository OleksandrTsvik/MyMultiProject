using Application.Birthdays.Create;
using Application.Common.Authentication;
using Domain.Birthdays;

namespace Application.UnitTests.Birthdays;

public class CreateBirthdayCommandHandlerTests
{
    private readonly Mock<IBirthdayRepository> _birthdayRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;

    private readonly CreateBirthdayCommandHandler _handler;

    public CreateBirthdayCommandHandlerTests()
    {
        _birthdayRepositoryMock = new();
        _userContextMock = new();

        _handler = new CreateBirthdayCommandHandler(
            _birthdayRepositoryMock.Object,
            _userContextMock.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Test note.")]
    public async Task Handle_Should_ReturnSuccessResult_WhenCreateSucceeds(string? note)
    {
        // Arrange
        var command = new CreateBirthdayCommand("Oleksandr Tsvik", DateTime.UtcNow, note);

        // Act
        Result<Guid> result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Test note.")]
    public async Task Handle_Should_CallUserContextAndRepository_WhenCreateSucceeds(string? note)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateBirthdayCommand("Oleksandr Tsvik", DateTime.UtcNow, note);

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(userId);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _userContextMock.Verify(
            userContext => userContext.UserId,
            Times.Once);

        _birthdayRepositoryMock.Verify(
            birthdayRepository => birthdayRepository.InsertAsync(
                It.Is<Birthday>(birthday =>
                    birthday.UserId == userId &&
                    birthday.FullName == command.FullName &&
                    birthday.Date == command.Date &&
                    birthday.Note == command.Note),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
