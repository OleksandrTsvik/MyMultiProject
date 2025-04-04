using Application.Common.Messaging;
using Application.Common.Models;

namespace Application.Users.Get;

public sealed record GetUsersQuery(
    string? UserName,
    string? Email,
    int? PageNumber,
    int? PageSize) : IQuery<PagedList<UserResponse>>;
