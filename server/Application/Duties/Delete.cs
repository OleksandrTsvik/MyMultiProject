using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            Duty duty = await _context.Duties.FindAsync(request.Id);

            if (duty == null)
            {
                return null;
            }

            _context.Duties.Remove(duty);

            int result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return Result<Unit>.Failure("Failed to delete the duty");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}