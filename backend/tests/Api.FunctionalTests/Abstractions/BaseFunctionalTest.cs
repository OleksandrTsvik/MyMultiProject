using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Api.FunctionalTests.Abstractions;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>, IDisposable
{
    protected readonly IServiceScope Scope;

    protected readonly ApplicationDbContext DbContext;
    protected readonly HttpClient HttpClient;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();

        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        HttpClient = factory.CreateClient();
    }

    public virtual void Dispose()
    {
        Scope.Dispose();
        DbContext.Dispose();
        HttpClient.Dispose();
    }
}
