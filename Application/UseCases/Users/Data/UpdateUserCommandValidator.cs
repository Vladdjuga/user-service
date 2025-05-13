using FluentValidation;

namespace Application.UseCases.Users.Data;

public class UpdateUserCommandValidator:AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Dto.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{}|;:,.<>?])[^\s]{8,128}$")
            .WithMessage("Password must be 8-128 characters long, contain at least one uppercase letter, one lowercase letter, one digit, one special character, and no spaces.");

        RuleFor(x => x.Dto.Username)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9_]{3,32}$")
            .WithMessage("Username must be 3-32 characters long and can only contain letters, digits, and underscores.");

        RuleFor(x => x.Dto.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Dto.FirstName)
            .NotEmpty()
            .Matches(@"^[A-Z][a-zA-Z\-']{1,31}$")
            .WithMessage("First name must start with a capital letter and contain only letters, hyphens, or apostrophes (max 32 chars).");

        RuleFor(x => x.Dto.LastName)
            .NotEmpty()
            .Matches(@"^[A-Z][a-zA-Z\-']{1,31}$")
            .WithMessage("Last name must start with a capital letter and contain only letters, hyphens, or apostrophes (max 32 chars).");

        RuleFor(x => x.Dto.DateOfBirth)
            .NotEmpty()
            .LessThan(new DateTime(2010, 1, 1))
            .GreaterThan(new DateTime(1900, 1, 1))
            .WithMessage("Date of birth must be between 1900-01-01 and 2010-01-01.");
    }
}