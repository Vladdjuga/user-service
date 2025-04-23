using Application.Interfaces.Security;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Users.Auth;

public class RegisterUserHandler:IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    public RegisterUserHandler(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }
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
            PasswordHash = _passwordHasher.HashPassword(request.Password),
        };
        await _repository.AddAsync(user,cancellationToken);
        return user.Id;
    }
}