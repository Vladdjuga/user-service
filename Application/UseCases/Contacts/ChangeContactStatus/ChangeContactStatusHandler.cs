using Application.Common;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Contacts.ChangeContactStatus;

public class ChangeContactStatusHandler:IRequestHandler<ChangeContactStatusCommand,Result>
{
    private readonly IUserContactRepository _userContactRepository;
    private readonly IMapper _mapper;

    public ChangeContactStatusHandler(IUserContactRepository userContactRepository, IMapper mapper)
    {
        _userContactRepository = userContactRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(ChangeContactStatusCommand request, CancellationToken cancellationToken)
    {
        var userContact = await _userContactRepository.GetUserContactAsync(request.UserId,
            request.ContactId,
            cancellationToken);
        if(userContact is null)
            return Result.Failure("User contact not found");
        await _userContactRepository.ChangeStatusAsync(userContact.Id,request.ContactStatus,cancellationToken);
        return Result.Success();
    }
}