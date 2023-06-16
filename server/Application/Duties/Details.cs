using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Details
{
    public class Query : IRequest<Result<Duty>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Duty>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Duty>> Handle(Query request, CancellationToken cancellationToken)
        {
            Duty duty = await _context.Duties.FindAsync(request.Id);

            return Result<Duty>.Success(duty);
        }
    }
}