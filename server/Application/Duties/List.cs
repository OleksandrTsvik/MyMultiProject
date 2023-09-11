using Application.Core;
using Application.Duties.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Application.Profiles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Duties;

public class List
{
    public class Query : IRequest<Result<PagedList<DutyDto>>>
    {
        public DutyParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<DutyDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<PagedList<DutyDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            QueryUser user = await _context.Users
                .Where(x => x.UserName == _userAccessor.GetUserName())
                .Select(x => x.ToQueryUser())
                .FirstOrDefaultAsync();

            IQueryable<DutyDto> query = _context.Duties
                .Where(x => x.AppUser.Id == user.Id)
                .Select(x => new DutyDto
                {
                    Id = x.Id,
                    Position = x.Position,
                    Title = x.Title,
                    Description = x.Description,
                    IsCompleted = x.IsCompleted,
                    DateCreation = x.DateCreation,
                    DateCompletion = x.DateCompletion,
                    BackgroundColor = x.BackgroundColor,
                    FontColor = x.FontColor
                })
                .AsQueryable();

            if (request.Params.IsCompleted)
            {
                query = query
                    .Where(x => x.IsCompleted)
                    .OrderBy(x => x.DateCompletion);
            }
            else
            {
                query = query
                    .Where(x => !x.IsCompleted)
                    .OrderBy(x => x.Position);
            }

            /* Title | DateCreation | BackgroundColor | FontColor */

            PagedList<DutyDto> duties = await PagedList<DutyDto>
                .CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

            return Result<PagedList<DutyDto>>.Success(duties);
        }
    }
}
