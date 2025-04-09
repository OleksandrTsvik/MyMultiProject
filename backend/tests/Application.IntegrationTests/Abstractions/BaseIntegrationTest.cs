using Application.Common.Authentication;
using Application.IntegrationTests.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.IntegrationTests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    protected readonly IServiceScope Scope;

    protected readonly HttpClient HttpClient;
    protected readonly ApplicationDbContext DbContext;
    protected readonly AuthenticationService AuthenticationService;
    protected readonly ISender Sender;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();

        HttpClient = factory.CreateClient();
        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Sender = Scope.ServiceProvider.GetRequiredService<ISender>();

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
