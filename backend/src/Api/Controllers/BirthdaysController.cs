using Api.Abstractions;
using Api.Contracts.Birthdays;
using Application.Birthdays.Create;
using Application.Birthdays.Delete;
using Application.Birthdays.Get;
using Application.Birthdays.Update;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace API.Controllers;

public class BirthdaysController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetBirthdays(
        [FromQuery(Name = "p")] int? pageNumber,
        [FromQuery(Name = "ps")] int? pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetBirthdaysQuery(pageNumber, pageSize);

        Result<PagedList<BirthdayResponse>> result = await Sender.Send(query, cancellationToken);

        return HandleResult(result);
    }

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
