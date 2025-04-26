using Application.Common;
using Application.DTOs.Chat;
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
}