using Application.Duties;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DutiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetDuties()
    {
        return this.HandleResult(await Mediator.Send(new List.Query()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDuty(Guid id)
    {
        return this.HandleResult(await Mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDuty(Duty duty)
    {
        return this.HandleResult(await Mediator.Send(new Create.Command { Duty = duty }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditDuty(Guid id, Duty duty)
    {
        duty.Id = id;

        return this.HandleResult(await Mediator.Send(new Edit.Command { Duty = duty }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDuty(Guid id)
    {
        return this.HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPut("list")]
    public async Task<IActionResult> EditDuties(List<Duty> duties)
    {
        return this.HandleResult(await Mediator.Send(new EditList.Command { Duties = duties }));
    }
}