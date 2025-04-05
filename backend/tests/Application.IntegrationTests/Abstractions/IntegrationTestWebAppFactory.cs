using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace Application.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
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
        builder.UseSetting("ApplicationDb:ConnectionString", _dbContainer.GetConnectionString());

        builder.UseEnvironment("Development");
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
