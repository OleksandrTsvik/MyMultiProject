using Application.DictionaryItems.DTOs;
using FluentValidation;

namespace Application.DictionaryItems.Validators;

public class EditDtoValidator : AbstractValidator<EditDto>
{
    public EditDtoValidator()
    {
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.Translation).NotEmpty();
    }
}
