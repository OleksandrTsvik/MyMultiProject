using Api.Abstractions;
using Api.Contracts.Birthdays;
using Application.Birthdays.Create;
using Application.Birthdays.Delete;
using Application.Birthdays.Update;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace API.Controllers;

public class BirthdaysController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateBirthday(
        [FromBody] CreateBirthdayRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBirthdayCommand(request.FullName, request.Date, request.Note);

        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBirthday(
        Guid id,
        [FromBody] UpdateBirthdayRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBirthdayCommand(id, request.FullName, request.Date, request.Note);

        Result result = await Sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBirthday(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBirthdayCommand(id);

        Result result = await Sender.Send(command, cancellationToken);

        return HandleResult(result);
    }
}
