using Application.Common;
using Application.DTOs.Chat;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Chats.GetChats;

public record GetChatsByUserIdQuery(Guid UserId):IRequest<Result<IEnumerable<ReadChatDto>>>;