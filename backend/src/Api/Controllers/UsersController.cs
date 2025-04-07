using Api.Abstractions;
using Application.Common.Models;
using Application.Users.Get;
using Application.Users.Login;
using Domain.Users;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Api.Controllers;

public sealed class UsersController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        Result<LoginResponse> result = await Sender.Send(command, cancellationToken);

        return HandleResult(result);
    }

    [HasPermission(UserPermissionType.ReadUser)]
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery(Name = "u")] string? userName,
        [FromQuery(Name = "e")] string? email,
        [FromQuery(Name = "p")] int? pageNumber,
        [FromQuery(Name = "ps")] int? pageSize,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(userName, email, pageNumber, pageSize);

        Result<PagedList<UserResponse>> result = await Sender.Send(query, cancellationToken);

        return HandleResult(result);
    }
}
