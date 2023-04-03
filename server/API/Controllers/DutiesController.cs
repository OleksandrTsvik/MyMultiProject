using Application.Duties;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DutiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Duty>>> GetDuties()
    {
        return await Mediator.Send(new List.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Duty>> GetDuty(Guid id)
    {
        return await Mediator.Send(new Details.Query { Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> CreateDuty(Duty duty)
    {
        return Ok(await Mediator.Send(new Create.Command { Duty = duty }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditDuty(Guid id, Duty duty)
    {
        duty.Id = id;

        return Ok(await Mediator.Send(new Edit.Command { Duty = duty }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDuty(Guid id)
    {
        return Ok(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPut("list")]
    public async Task<IActionResult> EditDuties(List<Duty> duties)
    {
        return Ok(await Mediator.Send(new EditList.Command { Duties = duties }));
    }
}