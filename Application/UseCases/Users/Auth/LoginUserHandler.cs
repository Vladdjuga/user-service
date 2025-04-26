using System.Text.RegularExpressions;
using Application.Auth;
using Application.Common;
using Application.Interfaces.Security;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Users.Auth;

public class LoginUserHandler:IRequestHandler<LoginUserCommand, Result<string>>
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
    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
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
            return Result<string>.Failure("Invalid Identity");
        if(userEntity == null)
            return Result<string>.Failure("User not found");
        if(!_passwordHasher.VerifyPassword(request.Password, userEntity.PasswordHash))
            return Result<string>.Failure("Invalid Password");
        var token = _jwtProvider.GenerateToken(userEntity.Id,userEntity.Username,userEntity.Email.Address);
        return Result<string>.Success(token);
    }
}