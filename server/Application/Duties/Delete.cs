using Domain;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Duty duty = await _context.Duties.FindAsync(request.Id);

            _context.Duties.Remove(duty);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}