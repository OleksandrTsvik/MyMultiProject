using Application.Birthdays.DTOs;
using FluentValidation;

namespace Application.Birthdays.Validators;

public class EditDtoValidator : AbstractValidator<EditDto>
{
    public EditDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
    }
}
