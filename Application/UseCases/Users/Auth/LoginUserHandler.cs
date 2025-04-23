using System.Text.RegularExpressions;
using Application.Auth;
using Application.Interfaces.Security;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Users.Auth;

public class LoginUserHandler:IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var nicknamePattern = @"^[a-zA-Z0-9_]{3,16}$";
        UserEntity? userEntity = null;
        if (Regex.IsMatch(request.Identity, emailPattern))
        {
            userEntity=await _userRepository.GetByEmailAsync(request.Identity,cancellationToken);
        }
        else if (Regex.IsMatch(request.Identity, nicknamePattern))
        {
            userEntity = await _userRepository.GetByUserNameAsync(request.Identity,cancellationToken);
        }
        else
            throw new ApplicationException("Invalid Identity");
        if(userEntity == null)
            throw new ApplicationException("Invalid Identity");
        if(!_passwordHasher.VerifyPassword(request.Password, userEntity.PasswordHash))
            throw new ApplicationException("Invalid Password");
        var token = _jwtProvider.GenerateToken(userEntity.Id,userEntity.Username,userEntity.Email.Address);
        return token;
    }
}