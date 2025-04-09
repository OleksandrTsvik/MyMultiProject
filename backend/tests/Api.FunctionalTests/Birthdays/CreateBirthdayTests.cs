using Application.Birthdays.Create;

namespace Api.FunctionalTests.Birthdays;

public class CreateBirthdayTests : BaseFunctionalTest
{
    public static readonly string RequestUri = "api/birthdays";

    public CreateBirthdayTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("A", "Test note.")]
    public async Task Post_Should_ReturnBadRequest_WhenFullNameIsInvalid(
        string? fullName,
        string? note)
    {
        // Arrange
        var request = new CreateBirthdayRequest(fullName!, DateTime.UtcNow, note);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Contains(nameof(CreateBirthdayCommand.FullName), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Post_Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        string fullName = "Oleksandr Tsvik";
        DateTime date = DateTime.UtcNow;
        string note = "Test note.";

        var request = new CreateBirthdayRequest(fullName, date, note);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Guid birthdayId = await response.GetContentAsync<Guid>();

        Assert.NotEqual(birthdayId, Guid.Empty);
    }
}
