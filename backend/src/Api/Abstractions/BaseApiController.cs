using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Api.Abstractions;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??=
        HttpContext.RequestServices.GetRequiredService<ISender>();

    [NonAction]
    protected ActionResult HandleResult(Result result)
    {
        if (result is null)
        {
            return GetErrorResult(Error.NullResult);
        }

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return GetErrorResult(result.Error);
    }

    [NonAction]
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result is null)
        {
            return GetErrorResult(Error.NullResult);
        }

        if (result.IsSuccess)
        {
            return result.Value is null
                ? NoContent()
                : GetValueResult(result.Value);
        }

        return GetErrorResult(result.Error);
    }

    [NonAction]
    protected ObjectResult GetErrorResult(Error error)
    {
        int statusCode = GetStatusCode(error.Type);
        string message = GetErrorMessage(error);

        var errorResponse = new ErrorResponse(statusCode, error.Code, message);

        return StatusCode(statusCode, errorResponse);
    }

    [NonAction]
    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    [NonAction]
    private static string GetErrorMessage(Error error)
    {
        return error.Message;
    }

    [NonAction]
    private ActionResult GetValueResult<T>(T value) =>
        value switch
        {
            byte[] bytes => File(bytes, "application/octet-stream"),
            _ => Ok(value)
        };
}
