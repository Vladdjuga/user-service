using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[Route("api/[controller]")]
public class AuthController(IMediator mediator):Controller
{
    [HttpPost("login")]
    public async Task<IResult> Login()
    {
        return Results.Empty;
    }
}