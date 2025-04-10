using Application.Birthdays.Get;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.IntegrationTests.Birthdays;

public class GetBirthdaysTests : BaseBirthdayTest, IAsyncLifetime
{
    public static readonly string RequestUri = "api/birthdays";

    public GetBirthdaysTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync()
    {
        await DbContext.Birthdays.ExecuteDeleteAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Get_Should_ReturnOk_WhenQueryStringIsEmpty()
    {
        // Arrange
        await CreateManyBirthdaysAsync("Test 01", "Test 02");

        string requestUri = GenerateQueryString();

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<BirthdayResponse>? birthdays = await response.GetContentAsync<PagedList<BirthdayResponse>>();

        Assert.NotNull(birthdays);
        Assert.Equal(1, birthdays.CurrentPage);
        Assert.Equal(1, birthdays.TotalPages);
        Assert.Equal(2, birthdays.TotalItems);
        Assert.False(birthdays.HasPreviousPage);
        Assert.False(birthdays.HasNextPage);
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenTryingGetAnotherUsersBirthdays()
    {
        // Arrange
        await CreateManyBirthdaysAsync("Test 01", "Test 02");

        string requestUri = GenerateQueryString();

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync(
            "test@mail.com",
            "password",
            []);

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<BirthdayResponse>? birthdays = await response.GetContentAsync<PagedList<BirthdayResponse>>();

        Assert.NotNull(birthdays);
        Assert.Equal(1, birthdays.CurrentPage);
        Assert.Equal(0, birthdays.TotalPages);
        Assert.Equal(0, birthdays.TotalItems);
        Assert.False(birthdays.HasPreviousPage);
        Assert.False(birthdays.HasNextPage);

        Assert.Empty(birthdays.Items);
    }

    [Fact]
    public async Task Get_Should_ReturnOk_WhenQueryStringIsValid()
    {
        // Arrange
        int pageNumber = 2;
        int pageSize = 5;

        await CreateManyBirthdaysAsync(
            "Test 01",
            "Test 02",
            "Test 03",
            "Test 04",
            "Test 05",
            "Test 06",
            "Test 07");

        string requestUri = GenerateQueryString(
            pageNumber: pageNumber,
            pageSize: pageSize);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedList<BirthdayResponse>? birthdays = await response.GetContentAsync<PagedList<BirthdayResponse>>();

        Assert.NotNull(birthdays);
        Assert.Equal(pageNumber, birthdays.CurrentPage);
        Assert.Equal(2, birthdays.TotalPages);
        Assert.Equal(pageSize, birthdays.PageSize);
        Assert.Equal(7, birthdays.TotalItems);
        Assert.True(birthdays.HasPreviousPage);
        Assert.False(birthdays.HasNextPage);
    }

    private static string GenerateQueryString(
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = new Dictionary<string, string?>()
        {
            ["p"] = pageNumber?.ToString(),
            ["ps"] = pageSize?.ToString(),
        };

        return QueryHelpers.AddQueryString(RequestUri, query);
    }
}
