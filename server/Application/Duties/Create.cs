using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Duties;

public class Create
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

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            _context.Duties.Add(request.Duty);

            int result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return Result<Unit>.Failure("Failed to create duty");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}