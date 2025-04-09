using Application.Birthdays.Create;
using Domain.Birthdays;

namespace Application.IntegrationTests.Birthdays;

public class CreateBirthdayCommandHandlerTests : BaseIntegrationTest
{
    public CreateBirthdayCommandHandlerTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Handle_Should_AddBirthday_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateBirthdayCommand("Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        // Act
        Result<Guid> result = await Sender.Send(command);

        // Assert
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == result.Value);

        Assert.NotNull(birthday);
        Assert.Equal(command.FullName, birthday.FullName);
        Assert.Equal(command.Date, birthday.Date);
        Assert.Equal(command.Note, birthday.Note);
    }
}
