using Application.Common.Authentication;
using Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddAuthentication()
            .AddQueries();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtBearerOptionsConfiguration>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddScoped<IUserContext, UserContext>();

        services.AddHttpContextAccessor();

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        return services;
    }
}
