using Application.Birthdays;
using Application.Birthdays.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BirthdaysController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetBirthdays()
    {
        return HandleResult(await Mediator.Send(new List.Query { }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBirthday(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBirthday(CreateDto birthday)
    {
        return HandleResult(await Mediator.Send(new Create.Command { CreateDto = birthday }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditBirthday(Guid id, EditDto birthday)
    {
        birthday.Id = id;

        return HandleResult(await Mediator.Send(new Edit.Command { EditDto = birthday }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBirthday(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }
}
