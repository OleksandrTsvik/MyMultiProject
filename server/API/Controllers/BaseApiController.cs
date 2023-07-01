using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    // protected IMediator Mediator => _mediator ??=
    //     HttpContext.RequestServices.GetService<IMediator>();

    protected IMediator Mediator
    {
        get
        {
            if (_mediator == null)
            {
                _mediator = HttpContext.RequestServices.GetService<IMediator>();
            }

            return _mediator;
        }
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null)
        {
            return NotFound();
        }

        if (result.IsSuccess && result.Value != null)
        {
            return Ok(result.Value);
        }

        if (result.IsSuccess && result.Value == null)
        {
            return NotFound();
        }

        if (result.ModelKey != null)
        {
            ModelState.AddModelError(result.ModelKey, result.Error);
            
            return ValidationProblem();
        }

        return BadRequest(result.Error);
    }
}