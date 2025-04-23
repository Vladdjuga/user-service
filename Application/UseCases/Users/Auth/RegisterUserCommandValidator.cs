using FluentValidation;

namespace Application.UseCases.Users.Auth;

public class RegisterUserCommandValidator:AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x=>x.Username).NotEmpty();
        RuleFor(x=>x.Email).NotEmpty();
        RuleFor(x=>x.FirstName).NotEmpty();
        RuleFor(x=>x.LastName).NotEmpty();
        RuleFor(x=>x.DateOfBirth).NotEmpty()
            .LessThan(new DateTime(2010,1,1))
            .GreaterThan(new DateTime(1900,1,1));
    }
}