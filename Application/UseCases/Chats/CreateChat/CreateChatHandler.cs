using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Chats.CreateChat;

public class CreateChatHandler:IRequestHandler<CreateChatCommand, Guid>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserChatRepository _userChatRepository;

    public CreateChatHandler(IChatRepository chatRepository
    , IUserRepository userRepository
    , IUserChatRepository userChatRepository)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _userChatRepository = userChatRepository;
    }
    public async Task<Guid> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new ChatEntity
        {
            Id = Guid.NewGuid(),
            Title = request.ChatName,
            IsPrivate = false,
            CreatedAt = DateTime.Now,
            ChatType = request.ChatType
        };
        await _chatRepository.AddAsync(chat);
        var userChat = new UserChatEntity
        {
            ChatId = chat.Id,
            UserId = request.CreatorId,
            ChatRole = ChatRole.Admin,
            IsMuted = false
        };
        await _userChatRepository.AddAsync(userChat);

        return chat.Id;
    }
}