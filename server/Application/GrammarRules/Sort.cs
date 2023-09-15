using Application.Core;
using Application.GrammarRules.DTOs;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GrammarRules;

public class Sort
{
    public class Command : IRequest<Result<Unit>>
    {
        public List<SortDto> SortDtos { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            List<Guid> ids = request.SortDtos.Select(x => x.Id).ToList();

            List<GrammarRule> rules = await _context.GrammarRules
                .Where(x => x.AppUser.UserName == _userAccessor.GetUserName() && ids.Contains(x.Id))
                .ToListAsync();

            foreach (GrammarRule rule in rules)
            {
                SortDto sortedRule = request.SortDtos.FirstOrDefault(x => x.Id == rule.Id);

                if (sortedRule != null)
                {
                    rule.Position = sortedRule.Position;
                }
            }

            bool result = await _context.SaveChangesAsync() >= 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to sort grammar rules");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
