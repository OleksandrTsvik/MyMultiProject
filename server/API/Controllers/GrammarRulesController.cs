using Application.GrammarRules;
using Application.GrammarRules.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/dictionary/rules")]
public class GrammarRulesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetRules()
    {
        return HandleResult(await Mediator.Send(new List.Query { }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRule(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRule(CreateDto category)
    {
        return HandleResult(await Mediator.Send(new Create.Command { CreateDto = category }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditRule(Guid id, EditDto category)
    {
        category.Id = id;

        return HandleResult(await Mediator.Send(new Edit.Command { EditDto = category }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRule(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPatch]
    public async Task<IActionResult> SortRules(List<SortDto> rules)
    {
        return HandleResult(await Mediator.Send(new Sort.Command { SortDtos = rules }));
    }
}
