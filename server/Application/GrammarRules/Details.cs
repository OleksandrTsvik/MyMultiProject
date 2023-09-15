using Application.Core;
using Application.GrammarRules.DTOs;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GrammarRules;

public class Details
{
    public class Query : IRequest<Result<GrammarRuleDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<GrammarRuleDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<GrammarRuleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            GrammarRuleDto rule = await _context.GrammarRules
                .Where(x => x.Id == request.Id && x.AppUser.UserName == _userAccessor.GetUserName())
                .Select(x => x.ToGrammarRuleDto())
                .FirstOrDefaultAsync();

            return Result<GrammarRuleDto>.Success(rule);
        }
    }
}
