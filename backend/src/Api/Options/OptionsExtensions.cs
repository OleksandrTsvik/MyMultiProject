using Api.Options.Models;
using FluentValidation;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Persistence.Options;

namespace Api.Options;

public static class OptionsExtensions
{
    public static IServiceCollection AddOptionsWithValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        services.AddOptionsWithFluentValidation<ApplicationDbOptions>(ApplicationDbOptions.ConfigurationSectionName);

        services.AddOptionsWithFluentValidation<CorsOptions>(CorsOptions.ConfigurationSectionName);

        services.AddOptionsWithFluentValidation<JwtOptions>(JwtOptions.ConfigurationSectionName);

        services.AddOptionsWithFluentValidation<SeedOptions>(SeedOptions.ConfigurationSectionName);

        return services;
    }

    private static IServiceCollection AddOptionsWithFluentValidation<TOptions>(
        this IServiceCollection services,
        string configurationSection)
        where TOptions : class
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(configurationSection)
            .ValidateFluentValidation()
            .ValidateOnStart();

        return services;
    }

    private static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider =>
            new FluentValidateOptions<TOptions>(serviceProvider, builder.Name));

        return builder;
    }
}
