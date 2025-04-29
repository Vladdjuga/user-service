using Application.Common;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Chats.AddUserToChat;

public record AddUserToChatCommand(Guid ChatId,Guid UserId,ChatRole ChatRole):IRequest<IResult>;