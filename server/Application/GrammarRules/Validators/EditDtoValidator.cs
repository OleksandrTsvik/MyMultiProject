using Application.GrammarRules.DTOs;
using FluentValidation;

namespace Application.GrammarRules.Validators;

public class EditDtoValidator : AbstractValidator<EditDto>
{
    public EditDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
    }
}

