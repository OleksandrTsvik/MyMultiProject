using FluentValidation;

namespace Application.DictionaryCategories;

public class CreateDictionaryCategoryValidator : AbstractValidator<CreateDto>
{
    public CreateDictionaryCategoryValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
        RuleFor(x => x.Position).NotNull();
    }
}
