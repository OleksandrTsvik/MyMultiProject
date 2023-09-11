using Application.DictionaryItems.DTOs;
using FluentValidation;

namespace Application.DictionaryItems.Validators;

public class CreateDtoValidator : AbstractValidator<CreateDto>
{
    public CreateDtoValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.Translation).NotEmpty();
    }
}
