using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            request.Duty.AppUser = user;

            Duty duty = await _context.Duties.FindAsync(request.Duty.Id);

            if (duty == null)
            {
                return null;
            }

            duty.Update(request.Duty);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to update duty");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
