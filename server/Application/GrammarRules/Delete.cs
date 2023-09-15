using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.GrammarRules;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
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
            GrammarRule rule = await _context.GrammarRules
                .FirstOrDefaultAsync(x => x.Id == request.Id &&
                    x.AppUser.UserName == _userAccessor.GetUserName());

            if (rule == null)
            {
                return null;
            }

            _context.GrammarRules.Remove(rule);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to delete the grammar rule");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
