using Application.Birthdays.DTOs;
using Application.Birthdays.Validators;
using Application.Core;
using Application.Interfaces;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Birthdays;

public class Create
{
    public class Command : IRequest<Result<BirthdayDto>>
    {
        public CreateDto CreateDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CreateDto).SetValidator(new CreateDtoValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<BirthdayDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<BirthdayDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null)
            {
                return null;
            }

            CreateDto addedBirthday = request.CreateDto;

            Birthday birthday = new Birthday
            {
                FullName = addedBirthday.FullName,
                Date = addedBirthday.Date,
                Note = addedBirthday.Note,
                AppUser = user
            };

            _context.Birthdays.Add(birthday);

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<BirthdayDto>.Failure("Failed to add birthday");
            }

            return Result<BirthdayDto>.Success(birthday.ToBirthdayDto());
        }
    }
}
