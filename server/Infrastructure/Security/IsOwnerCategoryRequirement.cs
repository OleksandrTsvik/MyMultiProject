using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsOwnerCategoryRequirement : IAuthorizationRequirement
{

}

public class IsOwnerCategoryRequirementHandler : AuthorizationHandler<IsOwnerCategoryRequirement>
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsOwnerCategoryRequirementHandler(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerCategoryRequirement requirement)
    {
        string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Task.CompletedTask;
        }

        Guid categoryId = Guid.Parse(_httpContextAccessor.HttpContext?.Request
            .RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString());

        DictionaryCategory category = _dataContext.DictionaryCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AppUser.Id == userId && x.Id == categoryId)
            .Result;

        if (category == null)
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
