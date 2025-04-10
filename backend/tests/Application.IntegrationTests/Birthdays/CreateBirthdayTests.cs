using Api.Contracts.Birthdays;
using Application.Birthdays.Create;
using Domain.Birthdays;

namespace Application.IntegrationTests.Birthdays;

public class CreateBirthdayTests : BaseIntegrationTest
{
    public static readonly string RequestUri = "api/birthdays";

    public CreateBirthdayTests(IntegrationTestWebAppFactory factory)
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

        ErrorResponse<List<ValidationError>> errorResponse = await response.GetValidationErrorResponseAsync();

        Assert.Equal(StatusCodes.Status400BadRequest, errorResponse.StatusCode);
        Assert.NotNull(errorResponse.Details);
        Assert.Contains(nameof(CreateBirthdayCommand.FullName), errorResponse.Details.Select(error => error.Code));
    }

    [Fact]
    public async Task Post_Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateBirthdayRequest("Oleksandr Tsvik", DateTime.UtcNow, "Test note.");

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Guid birthdayId = await response.GetContentAsync<Guid>();

        Assert.NotEqual(birthdayId, Guid.Empty);

        Birthday? birthday = await DbContext.Birthdays
            .FirstOrDefaultAsync(birthday => birthday.Id == birthdayId);

        Assert.NotNull(birthday);
        Assert.Equal(request.FullName, birthday.FullName);
        Assert.Equal(request.Note, birthday.Note);
    }
}
