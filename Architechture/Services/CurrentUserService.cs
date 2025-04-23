using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CurrentUserService:ICurrentUserService
{
    public Guid UserId { get; }
    public string Username { get; }
    public string Email { get; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            UserId = new Guid(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            Username = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }
}