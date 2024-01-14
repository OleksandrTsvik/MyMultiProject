using Application.Birthdays.DTOs;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Birthdays;

public class List
{
    public class Query : IRequest<Result<List<BirthdayDto>>> { }

    public class Handler : IRequestHandler<Query, Result<List<BirthdayDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<BirthdayDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Birthday> birthdays = await _context.Birthdays
                .Where(x => x.AppUser.UserName == _userAccessor.GetUserName())
                .ToListAsync();

            List<BirthdayDto> birthdayItems = birthdays
                .Select(x => new BirthdayDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Date = x.Date,
                    Note = x.Note,
                    Age = BirthdayUtil.GetAgeFromBirthday(x.Date),
                    DaysUntilBirthday = BirthdayUtil.GetDaysUntilBirthday(x.Date)
                })
                .OrderBy(x => x.DaysUntilBirthday)
                .ToList();

            return Result<List<BirthdayDto>>.Success(birthdayItems);
        }
    }
}
