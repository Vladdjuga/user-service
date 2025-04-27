using Application.Common;
using Application.DTOs.Contact;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Contacts.CreateContact;

public class CreateContactHandler:IRequestHandler<CreateContactCommand,Result<ReadContactDto>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserChatRepository _userChatRepository;
    private readonly IUserContactRepository _userContactRepository;
    private readonly IMapper _mapper;

    public CreateContactHandler(IChatRepository chatRepository, IUserRepository userRepository,
        IUserContactRepository userContactRepository, IUserChatRepository userChatRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _userContactRepository = userContactRepository;
        _userChatRepository = userChatRepository;
        _mapper = mapper;
    }
    public async Task<Result<ReadContactDto>> Handle(CreateContactCommand request,
        CancellationToken cancellationToken)
    {
        var userContact = await _userContactRepository.GetUserContactAsync(request.UserId, request.ContactId, cancellationToken);
        if(userContact is not null)
            return Result<ReadContactDto>.Failure("User contact already exists");
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<ReadContactDto>.Failure("User does not exist");
        var contact = await _userRepository.GetByIdAsync(request.ContactId, cancellationToken);
        if (contact is null)
            return Result<ReadContactDto>.Failure("Contact does not exist");
        var privateChat = new ChatEntity
        {
            Id = Guid.NewGuid(),
            Title = "Direct chat between "+user.Username+" and "+contact.Username,
            CreatedAt = DateTime.UtcNow,
            IsPrivate = true,
            ChatType = ChatType.Direct
        };
        await _chatRepository.AddAsync(privateChat, cancellationToken);
        var userChat = new UserChatEntity
        {
            ChatId = privateChat.Id,
            UserId = request.UserId,
            ChatRole = ChatRole.Admin,
            IsMuted = false
        };
        var contactChat = new UserChatEntity
        {
            ChatId = privateChat.Id,
            UserId = request.ContactId,
            ChatRole = ChatRole.Admin,
            IsMuted = false
        };
        await _userChatRepository.AddAsync(userChat,cancellationToken);
        await _userChatRepository.AddAsync(contactChat,cancellationToken);
        var userContactEntity = new UserContactEntity
        {
            Id=Guid.NewGuid(),
            UserId = request.UserId,
            ContactId = request.ContactId,
            ContactStatus = ContactStatus.Active,
            CreatedAt = DateTime.UtcNow,
            PrivateChatId = privateChat.Id
        };
        await _userContactRepository.AddAsync(userContactEntity, cancellationToken);
        var loadedUserContact = await _userContactRepository.GetByIdAsync(userContactEntity.Id, cancellationToken, 
            include => include
                .Include(x => x.Contact)
        );
        var dto=_mapper.Map<ReadContactDto>(loadedUserContact);
        return Result<ReadContactDto>.Success(dto);
    }
}