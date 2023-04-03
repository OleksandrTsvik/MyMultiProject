using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Duties;

public class EditList
{
    public class Command : IRequest
    {
        public List<Duty> Duties { get; set; }
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
            List<Guid> ids = request.Duties.Select(d => d.Id).ToList();

            List<Duty> duties = _context.Duties
                .Where(duty => ids.Contains(duty.Id))
                .ToList();

            duties.ForEach((duty) =>
            {
                Duty updatedDuty = request.Duties.FirstOrDefault(d => d.Id == duty.Id);

                if (updatedDuty != null)
                {
                    _mapper.Map(updatedDuty, duty);
                }
            });

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}