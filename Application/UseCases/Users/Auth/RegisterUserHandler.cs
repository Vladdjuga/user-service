using Application.Interfaces.Security;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Users.Auth;

public class RegisterUserHandler(IUserRepository repository,IPasswordHasher passwordHasher):IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserEntity
        {
            Username = request.Username,
            Email = new Email(request.Email),
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            CreatedAt = DateTime.Now,
            PasswordHash = passwordHasher.HashPassword(request.Password),
        };
        await repository.AddAsync(user);
        return user.Id;
    }
}