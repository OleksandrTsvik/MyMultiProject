using Api.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class HealthCheckController : BaseApiController
{
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult HealthCheck()
    {
        return Ok($"Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
    }

    [HttpGet("logs")]
    public IActionResult Logs()
    {
        _logger.LogInformation("Log Information");
        _logger.LogDebug("Log Debug");
        _logger.LogWarning("Log Warning");
        _logger.LogError("Log Error");
        _logger.LogCritical("Log Critical");
        _logger.LogTrace("Log Trace");

        return Ok("Logs have been recorded.");
    }
}
