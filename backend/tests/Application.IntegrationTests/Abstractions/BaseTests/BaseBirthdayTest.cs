using Api.Contracts.Birthdays;

namespace Application.IntegrationTests.Abstractions.BaseTests;

public abstract class BaseBirthdayTest : BaseIntegrationTest
{
    protected BaseBirthdayTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    protected async Task<Guid> CreateBirthdayAsync(
        string fullName,
        string? note = null,
        DateTime? date = null)
    {
        var request = new CreateBirthdayRequest(
            fullName,
            date ?? DateTime.UtcNow,
            note);

        HttpClient.DefaultRequestHeaders.Authorization = await AuthenticationService.GetAuthenticationHeaderAsync();

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/birthdays", request);
        Guid birthdayId = await response.GetContentAsync<Guid>();

        return birthdayId;
    }

    protected async Task CreateManyBirthdaysAsync(string fullName, params string[] fullNames)
    {
        await CreateBirthdayAsync(fullName);

        foreach (string item in fullNames)
        {
            await CreateBirthdayAsync(item);
        }
    }
}
