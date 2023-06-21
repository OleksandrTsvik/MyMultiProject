using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsOwnerDutyRequirement : IAuthorizationRequirement
{
}

public class IsOwnerDutyRequirementHandler : AuthorizationHandler<IsOwnerDutyRequirement>
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsOwnerDutyRequirementHandler(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerDutyRequirement requirement)
    {
        string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Task.CompletedTask;
        }

        Guid dutyId = Guid.Parse(_httpContextAccessor.HttpContext?.Request
            .RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString());

        Duty duty = _dataContext.Duties
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AppUser.Id == userId && x.Id == dutyId)
            .Result;

        if (duty == null)
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}