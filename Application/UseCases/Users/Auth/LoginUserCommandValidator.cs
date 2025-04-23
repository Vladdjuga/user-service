using FluentValidation;

namespace Application.UseCases.Users.Auth;

public class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(o=>o.Identity)
            .NotNull()
            .NotEmpty()
            .WithMessage("Identity field is required");
        RuleFor(o=>o.Password)
            .NotNull()
            .NotEmpty()
            .WithMessage("Password field is required")
            .MinimumLength(8)
            .WithMessage("Password must contain at least 8 characters");
    }
}