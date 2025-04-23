using Application.Auth;
using Application.Services;
using Domain.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Test;

[Route("api/[controller]")]
public class TokenController(IUserRepository userRepository,IJwtProvider jwtProvider) : Controller
{
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<string>> GetToken(Guid userId)
    {
        var user=await userRepository.GetByIdAsync(userId, CancellationToken.None);
        return jwtProvider.GenerateToken(user.Id,user.Username, user.Email.Address);
    }
}