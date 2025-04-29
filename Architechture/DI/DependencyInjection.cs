using Application;
using Application.Auth;
using Application.Behaviors;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Security;
using Application.Mappings;
using Application.Services;
using Application.Services.Security;
using Application.UseCases.Chats.CreateChat;
using Application.UseCases.Users.Data;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MessengerDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection"))
                .UseLazyLoadingProxies());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IChatRepository,ChatRepository>();
        services.AddScoped<IUserContactRepository,UserContactRepository>();
        services.AddScoped<IUserChatRepository,UserChatRepository>();
        services.AddTransient<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddValidatorsFromAssemblyContaining<GetUserQueryValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ExceptionHandlingBehavior<,>));
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(typeof(CreateChatHandler).Assembly);
        return services;
    }
}