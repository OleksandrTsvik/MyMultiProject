using Domain.Users;
using FluentValidation;
using Persistence.Options;

namespace Api.Options.Validators;

public sealed class SeedOptionsValidator : AbstractValidator<SeedOptions>
{
    public SeedOptionsValidator()
    {
        RuleForEach(x => x.Users).ChildRules(user =>
        {
            user.RuleFor(userSeed => userSeed.UserName)
                .NotEmpty()
                .MinimumLength(UserRules.MinUserNameLength)
                .MaximumLength(UserRules.MaxUserNameLength);

            user.RuleFor(userSeed => userSeed.Email)
                .EmailAddress()
                .MaximumLength(UserRules.MaxEmailLength);

            user.RuleFor(userSeed => userSeed.Password)
                .MinimumLength(UserRules.MinPasswordLength)
                .MaximumLength(UserRules.MaxPasswordLength);

            user.RuleFor(userSeed => userSeed.Roles).ForEach(role => role.NotEmpty());
        });
    }
}
