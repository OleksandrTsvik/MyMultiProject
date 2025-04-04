using Application.Common.Messaging;
using Application.Common.Models;
using Application.Users.Get;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Extensions;
using SharedKernel;

namespace Infrastructure.Queries.Users;

public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedList<UserResponse>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetUsersQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PagedList<UserResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<User> query = _dbContext.Users
            .WhereIf(
                !string.IsNullOrWhiteSpace(request.Email),
                user => EF.Functions.ILike(user.UserName, $"%{request.UserName}%"))
            .WhereIf(
                !string.IsNullOrWhiteSpace(request.Email),
                user => EF.Functions.ILike(user.Email, $"%{request.Email}%"));

        PagedList<UserResponse> users = await query
            .Select(user => new UserResponse(
                user.Id,
                user.UserName,
                user.Email,
                user.EmailVerified,
                user.Roles.Select(role => role.Name).ToArray()))
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return users;
    }
}
