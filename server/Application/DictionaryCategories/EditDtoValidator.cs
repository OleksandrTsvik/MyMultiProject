using FluentValidation;

namespace Application.DictionaryCategories;

public class EditDtoValidator : AbstractValidator<EditDto>
{
    public EditDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
    }
}
