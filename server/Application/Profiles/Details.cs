using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Application.Profiles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class Details
{
    public class Query : IRequest<Result<Profile>>
    {
        public string UserName { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Profile>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            Profile profile = await _context.Users
                .Where(x => x.UserName == request.UserName)
                    .Include(x => x.Followers)
                        .ThenInclude(f => f.Observer)
                    .Include(x => x.Followings)
                    .Include(x => x.Photos)
                .Select(x => x.ToProfile(_userAccessor.GetUserName()))
                .FirstOrDefaultAsync();

            return Result<Profile>.Success(profile);
        }
    }
}
