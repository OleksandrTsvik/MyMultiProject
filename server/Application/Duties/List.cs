using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Duties;

public class List
{
    public class Query : IRequest<Result<List<Duty>>> {}

    public class Handler : IRequestHandler<Query, Result<List<Duty>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Duty>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return Result<List<Duty>>.Success(await _context.Duties.ToListAsync());
        }
    }
}