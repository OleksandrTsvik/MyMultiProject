using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;

        public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<DutyDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            QueryUser user = await _context.Users
                .ProjectTo<QueryUser>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            IQueryable<DutyDto> query = _context.Duties
                .Where(x => x.AppUser.Id == user.Id)
                .ProjectTo<DutyDto>(_mapper.ConfigurationProvider)
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