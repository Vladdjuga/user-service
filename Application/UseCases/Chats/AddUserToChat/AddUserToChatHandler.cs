using Application.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Chats.AddUserToChat;

public class AddUserToChatHandler:IRequestHandler<AddUserToChatCommand,IResult>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserChatRepository _userChatRepository;

    public AddUserToChatHandler(IChatRepository chatRepository, IUserRepository userRepository, IUserChatRepository userChatRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _userChatRepository = userChatRepository;
    }

    public async Task<IResult> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
    {
        if(!await _userRepository.AnyByIdAsync(request.UserId, cancellationToken))
            return Result.Failure("Failed to find user");
        if(!await _chatRepository.AnyByIdAsync(request.ChatId, cancellationToken))
            return Result.Failure("Failed to find chat");
        if(await _userChatRepository.AnyByIdAsync(request.UserId,request.ChatId, cancellationToken))
            return Result.Failure("Failed to find user chat");
        var entity = new UserChatEntity
        {
            ChatId = request.ChatId,
            UserId = request.UserId,
            ChatRole = request.ChatRole,
            IsMuted = false
        };
        await _userChatRepository.AddAsync(entity, cancellationToken);
        return Result.Success();
    }
}