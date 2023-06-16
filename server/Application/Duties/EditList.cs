using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Duties;

public class EditList
{
    public class Command : IRequest<Result<Unit>>
    {
        public List<Duty> Duties { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleForEach(x => x.Duties).SetValidator(new DutyValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
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

            int result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return Result<Unit>.Failure("Failed to update duties");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}