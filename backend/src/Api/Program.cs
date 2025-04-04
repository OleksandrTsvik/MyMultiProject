using Api.Extensions;
using Api.Options;
using Application;
using Infrastructure;
using Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .ConfigureControllers()
    .AddExceptionHandler()
    .AddOptionsWithValidation()
    .AddApiCors(builder.Configuration)
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence();

WebApplication app = builder.Build();

await app.ConfigureDatabaseAsync();

app.UseApiCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
