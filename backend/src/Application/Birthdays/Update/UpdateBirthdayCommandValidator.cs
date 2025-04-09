using Application.Common.Extensions;
using Domain.Birthdays;
using FluentValidation;

namespace Application.Birthdays.Update;

public sealed class UpdateBirthdayCommandValidator : AbstractValidator<UpdateBirthdayCommand>
{
    public UpdateBirthdayCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MinimumLength(BirthdayRules.MinFullNameLength)
            .MaximumLength(BirthdayRules.MaxFullNameLength)
            .TrimWhitespace();
    }
}
