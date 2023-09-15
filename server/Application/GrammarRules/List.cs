using Application.Core;
using Application.GrammarRules.DTOs;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GrammarRules;

public class List
{
    public class Query : IRequest<Result<List<GrammarRuleDto>>> { }

    public class Handler : IRequestHandler<Query, Result<List<GrammarRuleDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<GrammarRuleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GrammarRuleDto> categories = await _context.GrammarRules
                .Where(x => x.AppUser.UserName == _userAccessor.GetUserName())
                .OrderBy(x => x.Position)
                .Select(x => x.ToGrammarRuleDto())
                .ToListAsync();

            return Result<List<GrammarRuleDto>>.Success(categories);
        }
    }
}
