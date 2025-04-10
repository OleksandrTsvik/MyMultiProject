using Api.Contracts;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Middleware;

public static class InvalidModelStateResponseHandler
{
    public static IActionResult Handle(ActionContext context)
    {
        IServiceProvider serviceProvider = context.HttpContext.RequestServices;
        ILogger<ActionContext> logger = serviceProvider.GetRequiredService<ILogger<ActionContext>>();

        int statusCode = StatusCodes.Status400BadRequest;
        string code = "InvalidModelState";
        string message = "Bad Request. Please check your data and try again.";

        List<ValidationError> validationErrors = GetModelStateErrors(context.ModelState);
        LogValidationErrors(logger, validationErrors);

        var errorResponse = new ErrorResponse(statusCode, code, message, validationErrors);

        return new ObjectResult(errorResponse)
        {
            StatusCode = statusCode
        };
    }

    private static List<ValidationError> GetModelStateErrors(ModelStateDictionary modelState)
    {
        var errors = new List<ValidationError>();

        foreach (string key in modelState.Keys)
        {
            ModelStateEntry? modelStateValue = modelState[key];

            if (modelStateValue is null)
            {
                continue;
            }

            var validationErrors = modelStateValue.Errors
                .Select(modelError => new ValidationError(key, modelError.ErrorMessage))
                .ToList();

            errors.AddRange(validationErrors);
        }

        return errors;
    }

    private static void LogValidationErrors(ILogger<ActionContext> logger, List<ValidationError> errors)
    {
        foreach (ValidationError error in errors)
        {
            logger.LogWarning("Validation error [{Code}]: {Message}", error.Code, error.Message);
        }
    }
}
