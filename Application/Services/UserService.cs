using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Security;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class UserService:IService<ReadUserDto,CreateUserDto,UpdateUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
        IUserRepository userRepository,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ReadUserDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        var dto = _mapper.Map<ReadUserDto>(user);
        return dto;
    }

    public async Task<IEnumerable<ReadUserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        if (users == null)
        {
            throw new Exception("No users found");
        }
        var dtos = _mapper.Map<IEnumerable<ReadUserDto>>(users);
        return dtos;
    }

    public async Task<ReadUserDto> CreateAsync(CreateUserDto dto)
    {
        var user = _mapper.Map<UserEntity>(dto);
        user.PasswordHash = _passwordHasher.HashPassword(dto.Password);
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<ReadUserDto>(user);
        return result;
    }

    public async Task<ReadUserDto> UpdateAsync(UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.Id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        _mapper.Map(dto, user);
        user.PasswordHash = _passwordHasher.HashPassword(dto.Password);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<ReadUserDto>(user);
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        await _userRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}