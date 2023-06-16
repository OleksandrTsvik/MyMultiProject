using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Duty Duty { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Duty).SetValidator(new DutyValidator());
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
            Duty duty = await _context.Duties.FindAsync(request.Duty.Id);

            if (duty == null)
            {
                return null;
            }

            _mapper.Map(request.Duty, duty);

            int result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return Result<Unit>.Failure("Failed to update duty");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}