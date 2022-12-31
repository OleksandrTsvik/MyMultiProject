using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Duties;

public class List
{
    public class Query : IRequest<List<Duty>> {}

    public class Handler : IRequestHandler<Query, List<Duty>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Duty>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Duties.ToListAsync();
        }
    }
}