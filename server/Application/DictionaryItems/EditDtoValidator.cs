using FluentValidation;

namespace Application.DictionaryItems;

public class EditDtoValidator : AbstractValidator<EditDto>
{
    public EditDtoValidator()
    {
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.Translation).NotEmpty();
    }
}
