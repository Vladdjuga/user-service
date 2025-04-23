using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.DTOs.User;
using Application.Interfaces.DTOs;
using Application.UseCases.Users.Data;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

/// <summary>
/// This is a REST controller that will manage mainly user data.
/// Changing and reading user data will happen here.
/// </summary>
[Route("api/[controller]")]
public class UserController:Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly HttpContext _httpContext;
    public UserController(IUserRepository userRepository, IMapper mapper, IMediator mediator,ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _httpContext = base.HttpContext;
    }
    /// <summary>
    /// This method will get info about 1 user only, finding them by username.
    /// </summary>
    /// <param name="username">The username string that will be passed by the client.</param>
    /// <returns>
    /// IReadUserDto or Bad Request if the exception was thrown or user was not found.
    /// The IReadUserDto instance can be either ReadUserDto or ReadUserPublicDto.
    /// If the JWT authorizing token is user`s then it will return ReadUserDto if not ReadUserPublicDto.
    /// </returns>
    [Authorize]
    [HttpGet("getUserInfo/{username}")]
    public async Task<Results<Ok<IReadUserDto>,UnauthorizedHttpResult,BadRequest<string>>> GetUserInfo(string username)
    {
        try
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                _logger.LogInformation("Couldn\'t parse from {UserIdString} to {UserIdGuid}"
                    ,userId, userGuid);
                return TypedResults.BadRequest("User not found");
            }
            var query = new GetUserQuery(
                Username:username,
                UserGuid:userGuid
            );
            _logger.LogInformation("Starting to get user info for {Username}", username);
            var result = await _mediator.Send(query);
            if (result is null)
            {
                _logger.LogInformation("User {Username} not found", username);
                return TypedResults.BadRequest("User not found");
            }
            _logger.LogInformation("User {Username} found", username);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user {Username} info. Message : {Message}",
                username, ex.Message);
            return TypedResults.BadRequest("Failed to get user info");
        }
    }
    /// <summary>
    /// This method will update a user in a PATCH manner, meaning it will only change what is new.
    /// </summary>
    /// <param name="userDto">This is a DTO where parameters that have to change will be passed. Alongside with the users ID</param>
    /// <returns>ReadUserDto or Bad Request if the exception was thrown.</returns>
    [Authorize]
    [HttpPatch("updateUserInfo")]
    public async Task<Results<Ok<ReadUserDto>,ForbidHttpResult, BadRequest<string>>> UpdateUserInfo(UpdateUserDto userDto)
    {
        try{
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                _logger.LogInformation("User {Username} not found", userGuid);
                return TypedResults.BadRequest("User not found");
            }
            var command = new UpdateUserCommand(userDto,userGuid);
            _logger.LogInformation("Starting to update user info for {Username}", userDto.Username);
            var result = await _mediator.Send(command);
            _logger.LogInformation("User {Username} updated", userDto.Username);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user info. Message : {Message}", ex.Message);
            return TypedResults.BadRequest("Failed to update user info");
        }
    }
}