using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransientEndsWith(this IServiceCollection services, string endsWith) =>
        services.AddEndsWith(endsWith, ServiceLifetime.Transient);

    public static IServiceCollection AddScopedEndsWith(this IServiceCollection services, string endsWith) =>
        services.AddEndsWith(endsWith, ServiceLifetime.Scoped);

    public static IServiceCollection AddSingletonEndsWith(this IServiceCollection services, string endsWith) =>
        services.AddEndsWith(endsWith, ServiceLifetime.Singleton);

    private static IServiceCollection AddEndsWith(
        this IServiceCollection services,
        string endsWith,
        ServiceLifetime lifetime)
    {
        var classes = AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith(endsWith))
            .ToList();

        foreach (Type @class in classes)
        {
            Type? @interface = @class.GetInterfaces().FirstOrDefault(type => type.Name.EndsWith(endsWith));

            if (@interface is not null)
            {
                services.Add(new ServiceDescriptor(@interface, @class, lifetime));
            }
        }

        return services;
    }

}
