using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Edit
{
    public class Command : IRequest
    {
        public Duty Duty { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Duty duty = await _context.Duties.FindAsync(request.Duty.Id);

            _mapper.Map(request.Duty, duty);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}