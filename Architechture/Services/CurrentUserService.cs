using System.Security.Claims;
using Application.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CurrentUserService:ICurrentUserService
{
    public required Guid UserId { get; init; }
    public required string Username { get;init; }
    public required string Email { get;init; }

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true) return;

        var tmp = GuidParser.SafeParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (tmp == null)
        {
            throw new ApplicationException("User is not authenticated");
        }
        UserId = tmp.Value;
        Username = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }
}