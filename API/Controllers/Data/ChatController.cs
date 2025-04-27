using System.IdentityModel.Tokens.Jwt;
using Application.Common;
using Application.DTOs.Chat;
using Application.UseCases.Chats.AddUserToChat;
using Application.UseCases.Chats.CreateChat;
using Application.UseCases.Chats.GetChats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Data;

[Route("api/[controller]")]
public class ChatController:Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ChatController(IMediator mediator, ILogger<ChatController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("getChats/{userId:Guid}")]
    public async Task<Results<Ok<IEnumerable<ReadChatDto>>,BadRequest<string>>> GetChats(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("UserId is empty");
            return TypedResults.BadRequest("UserId is empty");
        }

        var query = new GetChatsByUserIdQuery(userId);
        _logger.LogInformation("User {Username} tries to get chats", userId);
        var result = await _mediator.Send(query);
        if (result.IsFailure)
        {
            _logger.LogError("User {Username} failed to get chats", userId);
            _logger.LogError("Error : {Message}", result.Error);
            return TypedResults.BadRequest("Failed to get chats");
        }
        _logger.LogInformation("User {Username} gets the chats", userId);
        return TypedResults.Ok(result.Value);
    }

    [Authorize]
    [HttpPost("addChat")]
    public async Task<Results<Ok<ReadChatDto>,BadRequest<string>>> AddChat([FromBody] CreateChatDto createChatDto)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            _logger.LogInformation("User {Username} not found", userGuid);
            return TypedResults.BadRequest("User not found");
        }
        var command = new CreateChatCommand(
            createChatDto.Title,
            createChatDto.ChatType,
            userGuid
        );
        _logger.LogInformation("User {UserId} tries to create chat", userId);
        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogError("User {Username} failed to create chat", userId);
            _logger.LogError("Error : {Message}", result.Error);
            return TypedResults.BadRequest("Failed to create chat");
        }
        _logger.LogInformation("User {Username} added chat", userId);
        return TypedResults.Ok(result.Value);
    }

    [Authorize]
    [HttpPost("addUserToChat")]
    public async Task<Results<Ok, BadRequest<string>>> AddUserToChat([FromBody] AddUserToChatDto addUserToChatDto)
    {
        var command = new AddUserToChatCommand(
            addUserToChatDto.ChatId,
            addUserToChatDto.UserId,
            addUserToChatDto.ChatRole
        );
        _logger.LogInformation("Adding user {UserId} to chat {ChatId}",
            addUserToChatDto.UserId, addUserToChatDto.ChatId);
        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogError("Error while adding user {UserId} to chat {ChatId}",
                addUserToChatDto.UserId, addUserToChatDto.ChatId);
            _logger.LogError("Error : {Message}", result.Error);
            return TypedResults.BadRequest("Failed to create chat");
        }
        _logger.LogInformation("Added user {UserId} to chat {ChatId}",
            addUserToChatDto.UserId, addUserToChatDto.ChatId);
        return TypedResults.Ok();
    } 
    
}