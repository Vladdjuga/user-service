using Application.Common;
using Application.DTOs.User;
using Application.Interfaces.Security;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Users.Data;

public class UpdateUserHandler:IRequestHandler<UpdateUserCommand,Result<ReadUserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }
    public async Task<Result<ReadUserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if(request.Dto.Id!=request.UserId)
            return Result<ReadUserDto>.Failure("User does not exist");
        var userEntity = await _userRepository.GetByIdAsync(request.Dto.Id,cancellationToken);
        if (userEntity == null)
            return Result<ReadUserDto>.Failure("User not found");
        _mapper.Map(request.Dto, userEntity);
        if(request.Dto.Password!=null)
            userEntity.PasswordHash=_passwordHasher.HashPassword(request.Dto.Password);
        await _userRepository.UpdateAsync(userEntity,cancellationToken);
        return Result<ReadUserDto>.Success(_mapper.Map<ReadUserDto>(userEntity));
    }
}