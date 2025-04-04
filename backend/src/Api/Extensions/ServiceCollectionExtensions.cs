using System.Text.Json.Serialization;
using Api.Middleware;
using Api.Options.Models;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = InvalidModelStateResponseHandler.Handle;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddApiCors(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        CorsOptions? corsOptions = configuration
            .GetSection(CorsOptions.ConfigurationSectionName)
            .Get<CorsOptions>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .WithOrigins(corsOptions?.Origins ?? [])
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}
