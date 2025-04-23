using FluentValidation;

namespace Application.UseCases.Users.Data;

public class UpdateUserCommandValidator:AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        /*RuleFor(x => x.Dto.Password).NotEmpty();
        RuleFor(x=>x.Dto.Username).NotEmpty();
        RuleFor(x=>x.Dto.Email).NotEmpty();
        RuleFor(x=>x.Dto.FirstName).NotEmpty();
        RuleFor(x=>x.Dto.LastName).NotEmpty();
        RuleFor(x=>x.Dto.DateOfBirth).NotEmpty()
            .LessThan(new DateTime(2010,1,1))
            .GreaterThan(new DateTime(1900,1,1));
        RuleFor(x=>x.UserId).NotEmpty();*/
    }
}