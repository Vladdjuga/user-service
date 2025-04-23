using FluentValidation;

namespace Application.UseCases.Users.Data;

public class GetUserQueryValidator:AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
    }
}