using Api.Contracts.Birthdays;
using Domain.Birthdays;

namespace Application.IntegrationTests.Birthdays;

public class UpdateBirthdayTests : BaseBirthdayTest
{
    public static readonly string RequestUri = "api/birthdays";

    public UpdateBirthdayTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("A", "Test note.")]
    public async Task Put_Should_ReturnBadRequest_WhenFullNameIsInvalid(
        string? fullName,
        string? note)
    {
        // Arrange
        var birthdayId = Guid.NewGuid();

        var request = new UpdateBirthdayRequest(fullName!, DateTime.UtcNow, note);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{RequestUri}/{birthdayId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetValidationErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Contains(nameof(UpdateBirthdayRequest.FullName), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Put_Should_ReturnNotFound_WhenBirthdayNotFoundById()
    {
        // Arrange
        var birthdayId = Guid.NewGuid();

        var request = new UpdateBirthdayRequest("Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{RequestUri}/{birthdayId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        ErrorResponse errorResponse = await response.GetErrorResponseAsync();
        Error error = BirthdayErrors.NotFound();

        Assert.Equal(StatusCodes.Status404NotFound, errorResponse.StatusCode);
        Assert.Equal(error.Code, errorResponse.Code);
        Assert.Equal(error.Message, errorResponse.Message);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == birthdayId);

        Assert.Null(birthday);
    }

    [Fact]
    public async Task Put_Should_ReturnNotFound_WhenTryingUpdateAnotherUsersBirthday()
    {
        // Arrange
        string otherUserBirthdayFullName = "Oleksandr Tsvik";
        string otherUserBirthdayNote = "Test note.";
        Guid otherUserBirthdayId = await CreateBirthdayAsync(otherUserBirthdayFullName, otherUserBirthdayNote);

        var request = new UpdateBirthdayRequest("Test Name", DateTime.UtcNow, "Hello world.");

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync(
            "test@mail.com",
            "password",
            []);

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{RequestUri}/{otherUserBirthdayId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        ErrorResponse errorResponse = await response.GetErrorResponseAsync();
        Error error = BirthdayErrors.NotFound();

        Assert.Equal(StatusCodes.Status404NotFound, errorResponse.StatusCode);
        Assert.Equal(error.Code, errorResponse.Code);
        Assert.Equal(error.Message, errorResponse.Message);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == otherUserBirthdayId);

        Assert.NotNull(birthday);
        Assert.Equal(otherUserBirthdayFullName, birthday.FullName);
        Assert.Equal(otherUserBirthdayNote, birthday.Note);
    }

    [Fact]
    public async Task Put_Should_ReturnNoContent_WhenRequestIsValid()
    {
        // Arrange
        Guid birthdayId = await CreateBirthdayAsync("Oleksandr Tsvik");

        var request = new UpdateBirthdayRequest("Oleksandr Tsvik Dev", DateTime.UtcNow, "Test note.");

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{RequestUri}/{birthdayId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == birthdayId);

        Assert.NotNull(birthday);
        Assert.Equal(request.FullName, birthday.FullName);
        Assert.Equal(request.Note, birthday.Note);
    }
}
