using FluentValidation;

namespace Application.DictionaryCategories;

public class CreateDtoValidator : AbstractValidator<CreateDto>
{
    public CreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Language).NotEmpty();
        RuleFor(x => x.Position).NotNull();
    }
}
