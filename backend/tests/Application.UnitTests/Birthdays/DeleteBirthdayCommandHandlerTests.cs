using Application.Birthdays.Delete;
using Application.Common.Authentication;
using Domain.Birthdays;

namespace Application.UnitTests.Birthdays;

public class DeleteBirthdayCommandHandlerTests
{
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IBirthdayRepository> _birthdayRepositoryMock;

    private readonly DeleteBirthdayCommandHandler _handler;

    public DeleteBirthdayCommandHandlerTests()
    {
        _userContextMock = new();
        _birthdayRepositoryMock = new();

        _handler = new DeleteBirthdayCommandHandler(
            _userContextMock.Object,
            _birthdayRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenDeleteSucceeds()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var command = new DeleteBirthdayCommand(Guid.NewGuid());

        _userContextMock
            .Setup(userContext => userContext.UserId)
            .Returns(userId);

        _birthdayRepositoryMock.Setup(
            birthdayRepository => birthdayRepository.DeleteAsync(
                command.BirthdayId,
                userId,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        Result result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        _birthdayRepositoryMock.Verify(
            birthdayRepository => birthdayRepository.DeleteAsync(
                command.BirthdayId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
