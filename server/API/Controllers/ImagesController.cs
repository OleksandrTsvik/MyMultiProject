using Application.Core;
using Application.Images;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ImagesController : BaseApiController
{
    [HttpGet("{name}")]
    public async Task<IActionResult> GetImage(string name)
    {
        // TODO: solve 401 (Unauthorized)
        Result<byte[]> result = await Mediator.Send(new Details.Query { Name = name });

        return File(result.Value, "application/octet-stream");
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
