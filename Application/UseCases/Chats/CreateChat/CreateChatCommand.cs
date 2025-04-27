using Application.Common;
using Application.DTOs.Chat;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Chats.CreateChat;

public record CreateChatCommand(string ChatName,ChatType ChatType,Guid CreatorId):IRequest<Result<ReadChatDto>>;