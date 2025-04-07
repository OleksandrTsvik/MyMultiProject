using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application.IntegrationTests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    protected readonly IServiceScope Scope;

    protected readonly ApplicationDbContext DbContext;
    protected readonly ISender Sender;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();

        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Sender = Scope.ServiceProvider.GetRequiredService<ISender>();
    }

    public virtual void Dispose()
    {
        Scope.Dispose();
        DbContext.Dispose();
    }
}
