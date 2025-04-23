using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappings;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDto, UserEntity>()
            .ForMember(dest=>dest.PasswordHash,opt=>opt.MapFrom(src=>src.Password));
        //
        CreateMap<UserEntity, ReadUserDto>();
        CreateMap<UserEntity, ReadUserPublicDto>();
        //
        CreateMap<string, Email>().ConvertUsing(email => new Email(email));
        CreateMap<Email, string>().ConvertUsing(email => email.Address);
        //
        CreateMap<UpdateUserDto, UserEntity>()
            .ForMember(dest => dest.Email, opt =>
            {
                opt.PreCondition(src => src.Email != null);
                opt.MapFrom(src => new Email(src.Email!));
            })
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}