using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserEntity, ReadUserDto>();
    }
}