using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Application.Profiles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class List
{
    public class Query : IRequest<Result<List<Profile>>>
    {
        public string Predicate { get; set; }
        public string UserName { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<Profile>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<Profile> profiles = new List<Profile>();

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await _context.UserFollowings
                        .Where(x => x.Target.UserName == request.UserName)
                            .Include(x => x.Observer.Followers)
                                .ThenInclude(f => f.Observer)
                            .Include(x => x.Observer.Followings)
                            .Include(x => x.Observer.Photos)
                        .Select(x => x.Observer.ToProfile(_userAccessor.GetUserName()))
                        .ToListAsync();

                    break;
                case "following":
                    profiles = await _context.UserFollowings
                        .Where(x => x.Observer.UserName == request.UserName)
                            .Include(x => x.Target.Followers)
                                .ThenInclude(f => f.Observer)
                            .Include(x => x.Target.Followings)
                            .Include(x => x.Target.Photos)
                        .Select(x => x.Target.ToProfile(_userAccessor.GetUserName()))
                        .ToListAsync();

                    break;
            }

            return Result<List<Profile>>.Success(profiles);
        }
    }
}
