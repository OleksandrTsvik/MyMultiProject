using Domain;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Details
{
    public class Query : IRequest<Duty>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Duty>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Duty> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Duties.FindAsync(request.Id);
        }
    }
}