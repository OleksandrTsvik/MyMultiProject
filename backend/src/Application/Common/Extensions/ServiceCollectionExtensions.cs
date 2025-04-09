using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransientEndsWith(
        this IServiceCollection services,
        string endsWith,
        Assembly assembly)
    {
        return services.AddEndsWith(endsWith, ServiceLifetime.Transient, assembly);
    }

    public static IServiceCollection AddScopedEndsWith(
        this IServiceCollection services,
        string endsWith,
        Assembly assembly)
    {
        return services.AddEndsWith(endsWith, ServiceLifetime.Scoped, assembly);
    }

    public static IServiceCollection AddSingletonEndsWith(
        this IServiceCollection services,
        string endsWith,
        Assembly assembly)
    {
        return services.AddEndsWith(endsWith, ServiceLifetime.Singleton, assembly);
    }

    private static IServiceCollection AddEndsWith(
        this IServiceCollection services,
        string endsWith,
        ServiceLifetime lifetime,
        Assembly assembly)
    {
        var classes = assembly
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
