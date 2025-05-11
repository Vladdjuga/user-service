using Application.Common;
using Application.DTOs.Chat;
using Application.UseCases.Chats.GetChats;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Chats.AnyChatById;

public class AnyChatByIdHandler:IRequestHandler<AnyChatByIdQuery, Result<bool>>
{
    private readonly IUserChatRepository _userChatRepository;

    public AnyChatByIdHandler(IUserChatRepository userChatRepository)
    {
        _userChatRepository = userChatRepository;
    }

    public async Task<Result<bool>> Handle(AnyChatByIdQuery request,
        CancellationToken cancellationToken)
    {
        var res = await _userChatRepository.AnyByIdAsync(request.UserId,request.ChatId, cancellationToken);
        return !res ? Result<bool>.Failure("Did not find any user chat") : Result<bool>.Success(res);
    }
}