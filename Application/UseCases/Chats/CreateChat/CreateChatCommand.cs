using Application.Common;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Chats.CreateChat;

public record CreateChatCommand(string ChatName,ChatType ChatType,Guid CreatorId):IRequest<Result<Guid>>;