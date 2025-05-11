using Application.Common;
using MediatR;

namespace Application.UseCases.Chats.AnyChatById;

public record AnyChatByIdQuery(Guid UserId, Guid ChatId):IRequest<Result<bool>>;