using FluentValidation;

namespace Application.Users.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();

        RuleFor(x => x.Password).NotEmpty();
    }
}
