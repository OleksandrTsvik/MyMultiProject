using Application.Core;
using Application.Duties.DTOs;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Duties;

public class Details
{
    public class Query : IRequest<Result<DutyDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<DutyDto>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<DutyDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            DutyDto duty = await _context.Duties
                .Select(x => x.ToDutyDto())
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return Result<DutyDto>.Success(duty);
        }
    }
}
