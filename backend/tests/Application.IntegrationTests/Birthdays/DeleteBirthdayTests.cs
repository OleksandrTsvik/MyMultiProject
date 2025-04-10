using Domain.Birthdays;

namespace Application.IntegrationTests.Birthdays;

public class DeleteBirthdayTests : BaseBirthdayTest
{
    public static readonly string RequestUri = "api/birthdays";

    public DeleteBirthdayTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenBirthdayNotFoundById()
    {
        // Arrange
        var birthdayId = Guid.NewGuid();

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"{RequestUri}/{birthdayId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenTryingDeleteAnotherUsersBirthday()
    {
        // Arrange
        Guid otherUserBirthdayId = await CreateBirthdayAsync("Oleksandr Tsvik");

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync(
            "test@mail.com",
            "password",
            []);

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"{RequestUri}/{otherUserBirthdayId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == otherUserBirthdayId);

        Assert.NotNull(birthday);
    }

    [Fact]
    public async Task Delete_Should_ReturnNoContent_WhenRequestIsValid()
    {
        // Arrange
        Guid birthdayId = await CreateBirthdayAsync("Oleksandr Tsvik");

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"{RequestUri}/{birthdayId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == birthdayId);

        Assert.Null(birthday);
    }
}
