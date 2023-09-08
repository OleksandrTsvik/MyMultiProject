using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Duties;

public class Titles
{
    public class Query : IRequest<Result<List<string>>> { }

    public class Handler : IRequestHandler<Query, Result<List<string>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<string> titles = await _context.Duties
                .Where(x => x.AppUser.UserName == _userAccessor.GetUserName())
                .Select(x => x.Title)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            return Result<List<string>>.Success(titles);
        }
    }
}
