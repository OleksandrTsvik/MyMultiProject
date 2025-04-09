using Application.Common.Extensions;
using Domain.Birthdays;
using FluentValidation;

namespace Application.Birthdays.Create;

public sealed class CreateBirthdayCommandValidator : AbstractValidator<CreateBirthdayCommand>
{
    public CreateBirthdayCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(BirthdayRules.MinFullNameLength)
            .MaximumLength(BirthdayRules.MaxFullNameLength)
            .TrimWhitespace();
    }
}
