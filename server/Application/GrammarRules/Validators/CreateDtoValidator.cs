using Application.GrammarRules.DTOs;
using FluentValidation;

namespace Application.GrammarRules.Validators;

public class CreateDtoValidator : AbstractValidator<CreateDto>
{
    public CreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
    }
}
