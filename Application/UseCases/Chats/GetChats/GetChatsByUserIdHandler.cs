using Application.Common;
using Application.DTOs.Chat;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Chats.GetChats;

public class GetChatsByUserIdHandler:IRequestHandler<GetChatsByUserIdQuery, Result<IEnumerable<ReadChatDto>>>
{
    private readonly IUserChatRepository _userChatRepository;
    private readonly IMapper _mapper;

    public GetChatsByUserIdHandler(IUserChatRepository userChatRepository, IMapper mapper)
    {
        _userChatRepository = userChatRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ReadChatDto>>> Handle(GetChatsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _userChatRepository.GetChatsByUserIdAsync(request.UserId,
            true,
            cancellationToken);
        return Result<IEnumerable<ReadChatDto>>.Success(_mapper.Map<IEnumerable<ReadChatDto>>(result));
    }
}