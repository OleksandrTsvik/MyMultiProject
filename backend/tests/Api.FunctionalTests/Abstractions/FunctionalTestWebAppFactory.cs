using Api.FunctionalTests.Abstractions.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Api.FunctionalTests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("test_db")
        .WithUsername("root")
        .WithPassword("root")
        .WithCleanUp(true)
        .WithAutoRemove(true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.UseSetting("ApplicationDb:ConnectionString", _dbContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AnonymousAuthorizationHandler>();
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();

        await base.DisposeAsync();
    }
}
