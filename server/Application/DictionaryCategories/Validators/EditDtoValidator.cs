using Application.DictionaryCategories.DTOs;
using FluentValidation;

namespace Application.DictionaryCategories.Validators;

public class EditDtoValidator : AbstractValidator<EditDto>
{
    public EditDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
    }
}
