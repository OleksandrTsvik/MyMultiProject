using Application.Birthdays.DTOs;
using Application.Birthdays.Validators;
using Application.Core;
using Application.Mappers;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Birthdays;

public class Edit
{
    public class Command : IRequest<Result<BirthdayDto>>
    {
        public EditDto EditDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.EditDto).SetValidator(new EditDtoValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<BirthdayDto>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<BirthdayDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            EditDto editedBirthday = request.EditDto;

            Birthday birthday = await _context.Birthdays.FindAsync(editedBirthday.Id);

            if (birthday == null)
            {
                return null;
            }

            birthday.FullName = editedBirthday.FullName;
            birthday.Date = editedBirthday.Date;
            birthday.Note = editedBirthday.Note;

            bool result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return Result<BirthdayDto>.Failure("Failed to update birthday");
            }

            return Result<BirthdayDto>.Success(birthday.ToBirthdayDto());
        }
    }
}
