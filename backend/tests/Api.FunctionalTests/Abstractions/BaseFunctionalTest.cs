using Api.FunctionalTests.Abstractions.Services;
using Application.Common.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Api.FunctionalTests.Abstractions;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>, IDisposable
{
    protected readonly IServiceScope Scope;

    protected readonly HttpClient HttpClient;
    protected readonly ApplicationDbContext DbContext;
    protected readonly AuthenticationService AuthenticationService;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();

        HttpClient = factory.CreateClient();
        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        AuthenticationService = new AuthenticationService(
            HttpClient,
            DbContext,
            Scope.ServiceProvider.GetRequiredService<IPasswordHasher>());
    }

    public virtual void Dispose()
    {
        Scope.Dispose();
        HttpClient.Dispose();
        DbContext.Dispose();
    }
}
