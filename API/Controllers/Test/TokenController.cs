using Application.Auth;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Test;

[Route("api/[controller]")]
public class TokenController(UserService userService,IJwtProvider jwtProvider) : Controller
{
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<string>> GetToken(Guid userId)
    {
        var user=await userService.GetByIdAsync(userId);
        return jwtProvider.GenerateToken(user.Id,user.Username, user.Email);
    }
}