using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Abstractions;
using Persistence.Extensions;
using Persistence.Options;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services
            .AddApplicationDb()
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddApplicationDb(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            ApplicationDbOptions applicationDbOptions = serviceProvider
                .GetRequiredService<IOptions<ApplicationDbOptions>>().Value;

            options
                .UseNpgsql(applicationDbOptions.ConnectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<ApplicationDbInitializer>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScopedEndsWith("Repository");

        return services;
    }
}
