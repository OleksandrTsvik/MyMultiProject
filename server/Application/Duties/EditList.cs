using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            List<Guid> ids = request.Duties.Select(d => d.Id).ToList();

            List<Duty> duties = await _context.Duties
                .Where(x => x.AppUser.Id == user.Id && ids.Contains(x.Id))
                .ToListAsync();

            duties.ForEach((duty) =>
            {
                Duty updatedDuty = request.Duties.FirstOrDefault(d => d.Id == duty.Id);

                updatedDuty.AppUserId = user.Id;
                updatedDuty.AppUser = user;

                if (updatedDuty != null)
                {
                    _mapper.Map(updatedDuty, duty);
                }
            });

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<Unit>.Failure("Failed to update duties");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}