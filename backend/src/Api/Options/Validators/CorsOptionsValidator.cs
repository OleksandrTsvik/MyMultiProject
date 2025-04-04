using Api.Options.Models;
using FluentValidation;

namespace Api.Options.Validators;

public sealed class CorsOptionsValidator : AbstractValidator<CorsOptions>
{
    public CorsOptionsValidator()
    {
        RuleFor(x => x.Origins).NotEmpty();
    }
}
