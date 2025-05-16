using Application.UseCases.Chats.AnyChatById;
using Application.Utilities;
using Chat;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.gRPCClients;

public class ChatService:Chat.ChatService.ChatServiceBase
{
    private readonly ILogger<ChatService> _logger;
    private readonly IMediator _mediator;

    public ChatService(ILogger<ChatService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [Authorize]
    public override async Task<UserChatExistsResponse> UserChatExists(UserChatExistsRequest request,
        ServerCallContext context)
    {
        var userGuid = GuidParser.SafeParse(request.UserId);
        var chatGuid = GuidParser.SafeParse(request.ChatId);
        if (chatGuid == null || userGuid == null)
        {
            _logger.LogError("Invalid chatId or userGuid");
            return new UserChatExistsResponse { Exists = false };
        }
        var query = new AnyChatByIdQuery(
            userGuid.Value,
            chatGuid.Value
        );
        _logger.LogInformation("(gRPC) Checking if user is in chat");
        var result = await _mediator.Send(query);
        _logger.LogInformation("(gRPC) If user {UserId} is in chat {ChatId}. Result of query : {Bool}",
            userGuid.Value,chatGuid.Value,result);
        return new UserChatExistsResponse { Exists = result.Value };
    }
}