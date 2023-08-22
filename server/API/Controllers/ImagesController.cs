using Application.Core;
using Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ImagesController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetImage(string name)
    {
        Result<string> result = await Mediator.Send(new Details.Query { Name = name });

        return PhysicalFile(result.Value, "application/octet-stream");
    }

    [HttpPost]
    public async Task<IActionResult> AddImage([FromForm] Add.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }
}
