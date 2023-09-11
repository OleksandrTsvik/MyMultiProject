using Application.DictionaryCategories.DTOs;
using FluentValidation;

namespace Application.DictionaryCategories.Validators;

public class CreateDtoValidator : AbstractValidator<CreateDto>
{
    public CreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
    }
}
