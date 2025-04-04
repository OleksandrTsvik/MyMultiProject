using FluentValidation;
using Persistence.Options;

namespace Api.Options.Validators;

public sealed class ApplicationDbOptionsValidator : AbstractValidator<ApplicationDbOptions>
{
    public ApplicationDbOptionsValidator()
    {
        RuleFor(x => x.ConnectionString).NotEmpty();
    }
}
