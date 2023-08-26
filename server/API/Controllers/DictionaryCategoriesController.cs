using Application.DictionaryCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/dictionary/categories")]
public class DictionaryCategoriesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return HandleResult(await Mediator.Send(new List.Query { }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateDto category)
    {
        return HandleResult(await Mediator.Send(new Create.Command { CreateDto = category }));
    }

    [Authorize(Policy = "IsOwnerCategory")]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategory(Guid id, EditDto category)
    {
        category.Id = id;

        return HandleResult(await Mediator.Send(new Edit.Command { EditDto = category }));
    }

    [Authorize(Policy = "IsOwnerCategory")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPatch]
    public async Task<IActionResult> SortCategories(List<SortDto> categories)
    {
        return HandleResult(await Mediator.Send(new Sort.Command { SortDtos = categories }));
    }
}
