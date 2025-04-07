using Microsoft.AspNetCore.Authorization;

namespace Api.FunctionalTests.Abstractions.Authorization;

public sealed class AnonymousAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();

        foreach (IAuthorizationRequirement requirement in pendingRequirements)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
