using FluentValidation;

namespace Application.Common.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> TrimWhitespace<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => value is null || value.Length == value.Trim().Length)
            .WithMessage("Remove unnecessary spaces at the beginning or end.");
    }

    public static IRuleBuilderOptions<T, string?> IsUrl<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(value => value is not null && Uri.TryCreate(value, UriKind.Absolute, out _))
            .WithMessage("The link is invalid.");
    }
}
