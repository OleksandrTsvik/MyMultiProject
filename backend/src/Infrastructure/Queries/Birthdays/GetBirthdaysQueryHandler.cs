using Application.Birthdays.Get;
using Application.Common.Authentication;
using Application.Common.Messaging;
using Application.Common.Models;
using Domain.Birthdays;
using Persistence;
using Persistence.Extensions;
using SharedKernel;

namespace Infrastructure.Queries.Birthdays;

public sealed class GetBirthdaysQueryHandler : IQueryHandler<GetBirthdaysQuery, PagedList<BirthdayResponse>>
{
    private readonly IUserContext _userContext;
    private readonly ApplicationDbContext _dbContext;

    public GetBirthdaysQueryHandler(IUserContext userContext, ApplicationDbContext dbContext)
    {
        _userContext = userContext;
        _dbContext = dbContext;
    }

    public async Task<Result<PagedList<BirthdayResponse>>> Handle(
        GetBirthdaysQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Birthday> query = _dbContext.Birthdays
            .Where(birthday => birthday.UserId == _userContext.UserId);

        PagedList<Birthday> birthdays = await query
            .Select(birthday => new Birthday
            {
                Id = birthday.Id,
                FullName = birthday.FullName,
                Date = birthday.Date,
                Note = birthday.Note,
            })
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        var response = birthdays.Items
            .Select(birthday => new BirthdayResponse(
                birthday.Id,
                birthday.FullName,
                birthday.Date,
                birthday.Note,
                GetAgeFromBirthday(birthday.Date),
                GetDaysUntilBirthday(birthday.Date)))
            .ToList();

        return new PagedList<BirthdayResponse>(
            response,
            birthdays.TotalItems,
            birthdays.CurrentPage,
            birthdays.PageSize);
    }

    private static int GetAgeFromBirthday(DateTime birthdayDate)
    {
        DateTime today = DateTime.Today;

        int age = today.Year - birthdayDate.Year;

        if (today.Month < birthdayDate.Month ||
            (today.Month == birthdayDate.Month && today.Day < birthdayDate.Day))
        {
            age--;
        }

        return age;
    }

    private static int GetDaysUntilBirthday(DateTime birthdayDate)
    {
        DateTime today = DateTime.Today;
        var nextBirthdayDate = new DateTime(today.Year, birthdayDate.Month, birthdayDate.Day);

        if (nextBirthdayDate < today)
        {
            nextBirthdayDate = nextBirthdayDate.AddYears(1);
        }

        return (nextBirthdayDate - today).Days;
    }
}
