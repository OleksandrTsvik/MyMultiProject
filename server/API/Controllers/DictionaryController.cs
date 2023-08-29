using Application.Dictionary;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DictionaryController : BaseApiController
{
    [HttpGet("quantity")]
    public async Task<IActionResult> GetQuantity()
    {
        return HandleResult(await Mediator.Send(new Quantity.Query { }));
    }
}
