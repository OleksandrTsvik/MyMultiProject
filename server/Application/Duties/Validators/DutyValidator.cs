using Domain;
using FluentValidation;

namespace Application.Duties.Validators;

public class DutyValidator : AbstractValidator<Duty>
{
    public DutyValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.DateCreation).NotEmpty();
    }
}
