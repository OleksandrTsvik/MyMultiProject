using Application.Core;
using Application.GrammarRules.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GrammarRules;

public class List
{
    public class Query : IRequest<Result<List<GrammarRuleListItemDto>>> { }

    public class Handler : IRequestHandler<Query, Result<List<GrammarRuleListItemDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<GrammarRuleListItemDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GrammarRuleListItemDto> categories = await _context.GrammarRules
                .Where(x => x.AppUser.UserName == _userAccessor.GetUserName())
                .OrderBy(x => x.Position)
                .Select(x => new GrammarRuleListItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language,
                    Status = x.Status,
                    Position = x.Position,
                    DateCreation = x.DateCreation
                })
                .ToListAsync();

            return Result<List<GrammarRuleListItemDto>>.Success(categories);
        }
    }
}
