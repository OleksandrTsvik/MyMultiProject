using Application.Common.Messaging;
using Application.Common.Models;

namespace Application.Birthdays.Get;

public sealed record GetBirthdaysQuery(
    int? PageNumber,
    int? PageSize) : IQuery<PagedList<BirthdayResponse>>;
