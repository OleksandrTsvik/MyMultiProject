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
    public class Query : IRequest<Result<List<DutyDto>>> { }

    public class Handler : IRequestHandler<Query, Result<List<DutyDto>>>
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

        public async Task<Result<List<DutyDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            QueryUser user = await _context.Users
                .ProjectTo<QueryUser>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            List<DutyDto> duties = await _context.Duties
                .Where(x => x.AppUser.Id == user.Id)
                .ProjectTo<DutyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Result<List<DutyDto>>.Success(duties);
        }
    }
}