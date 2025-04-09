using Api.Abstractions;
using Application.Birthdays.Create;
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
}
