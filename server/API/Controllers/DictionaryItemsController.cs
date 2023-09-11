using Application.DictionaryItems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/dictionary/items")]
public class DictionaryItemsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetItems([FromQuery] DictionaryItemParams param)
    {
        return HandleResult(await Mediator.Send(new List.Query { Params = param }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(CreateDto item)
    {
        return HandleResult(await Mediator.Send(new Create.Command { CreateDto = item }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditItem(Guid id, EditDto item)
    {
        item.Id = id;

        return HandleResult(await Mediator.Send(new Edit.Command { EditDto = item }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPatch]
    public async Task<IActionResult> SortItems(List<SortDto> items)
    {
        return HandleResult(await Mediator.Send(new Sort.Command { SortDtos = items }));
    }
}
