using Application.Birthdays.DTOs;
using FluentValidation;

namespace Application.Birthdays.Validators;

public class CreateDtoValidator : AbstractValidator<CreateDto>
{
    public CreateDtoValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
    }
}
