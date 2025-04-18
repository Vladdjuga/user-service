using Application;
using Application.Auth;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Interfaces.Security;
using Application.Mappings;
using Application.Services;
using Application.Services.Security;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MessengerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection")));
        services.AddScoped<UserService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IChatRepository,ChatRepository>();
        services.AddScoped<IUserChatRepository,UserChatRepository>();
        services.AddTransient<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}