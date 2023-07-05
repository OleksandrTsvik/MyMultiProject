using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class List
{
    public class Query : IRequest<Result<List<Profiles.Profile>>>
    {
        public string Predicate { get; set; }
        public string UserName { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
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

        public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Profiles.Profile> profiles = new List<Profiles.Profile>();

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await _context.UserFollowings
                        .Where(x => x.Target.UserName == request.UserName)
                        .Select(x => x.Observer)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,
                            new { currentUserName = _userAccessor.GetUserName() })
                        .ToListAsync();

                    break;
                case "following":
                    profiles = await _context.UserFollowings
                        .Where(x => x.Observer.UserName == request.UserName)
                        .Select(x => x.Target)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,
                            new { currentUserName = _userAccessor.GetUserName() })
                        .ToListAsync();

                    break;
            }

            return Result<List<Profiles.Profile>>.Success(profiles);
        }
    }
}