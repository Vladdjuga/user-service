using Application.DTOs.User;
using Application.UseCases.Users.Auth;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

/// <summary>
/// This is an authorizing controller.
/// </summary>
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// This method will authorize the user to the application.
    /// </summary>
    /// <param name="loginDto">This is a login information.</param>
    /// <returns>Will return a generated JWT token or bad request if the exception was thrown.</returns>
    [HttpPost("login")]
    public async Task<Results<Ok<string>, BadRequest<string>>> Login([FromBody] LoginUserDto loginDto)
    {
        var command = new LoginUserCommand(
            Identity: loginDto.Identity,
            Password: loginDto.Password
        );
        _logger.LogInformation("Starting to login User: {Identity} - {Password}", loginDto.Identity, loginDto.Password);
        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogError("Failure while login user {User}.", loginDto.Identity);
            _logger.LogError("Error: {Error}", result.Error);
            return TypedResults.BadRequest(result.Error);
        }
        _logger.LogInformation("Login User: {Identity} - {Password}", loginDto.Identity, loginDto.Password);
        _logger.LogInformation("Token generated for user - {Identity}. Token : {Token}", loginDto.Identity, result);
        return TypedResults.Ok(result.Value);
    }

    /// <summary>
    /// This method will register a user.
    /// </summary>
    /// <param name="registerDto">This is a registration information.</param>
    /// <returns>Will return a newly added users Guid key or bad request if the exception was thrown.</returns>
    [HttpPost("register")]
    public async Task<Results<Ok<Guid>, BadRequest<string>>> Register([FromBody] RegisterUserDto registerDto)
    {
        var command = new RegisterUserCommand(
            Username: registerDto.Username,
            Email: registerDto.Email,
            Password: registerDto.Password,
            FirstName: registerDto.FirstName,
            LastName: registerDto.LastName,
            DateOfBirth: registerDto.DateOfBirth
        );
        _logger.LogInformation("Starting to register user {Username} - {Email}",
            registerDto.Username, registerDto.Email);
        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogError("Failure while registering user {Username} - {Email}", registerDto.Username, registerDto.Email);
            _logger.LogError("Error: {Error}", result.Error);
            return TypedResults.BadRequest(result.Error);
        }
        _logger.LogInformation("Registered User: {Username} - {Email}", registerDto.Username, registerDto.Email);
        return TypedResults.Ok(result.Value);
    }
}