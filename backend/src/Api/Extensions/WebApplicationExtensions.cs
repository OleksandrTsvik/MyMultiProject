using Persistence.Abstractions;

namespace Api.Extensions;

public static class WebApplicationExtensions
{
    public static void UseApiCors(this WebApplication app)
    {
        app.UseCors("CorsPolicy");
    }

    public static async Task ConfigureDatabaseAsync(this WebApplication app)
    {
        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();

        ApplicationDbInitializer applicationDbInitializer = scope.ServiceProvider
            .GetRequiredService<ApplicationDbInitializer>();

        await applicationDbInitializer.Execute();
    }
}
