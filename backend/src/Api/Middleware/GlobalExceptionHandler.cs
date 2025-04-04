using Api.Abstractions;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode = GetStatusCode(exception);
        string code = GetCode(exception);
        string message = GetMessage(exception);
        object? details = GetDetails(exception);

        var errorResponse = new ErrorResponse(statusCode, code, message, details);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            UserIdUnavailableException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetCode(Exception exception) =>
        exception switch
        {
            ValidationException => "Global.Validation",
            UserIdUnavailableException => "Global.UserIdUnavailable",
            _ => "Global"
        };

    private static string GetMessage(Exception exception) =>
        exception switch
        {
            ValidationException => "Bad Request. Please check your data and try again.",
            _ => "Internal Server Error."
        };

    private static object? GetDetails(Exception exception) =>
        exception switch
        {
            ValidationException ex => ex.Errors,
            _ => null
        };
}
