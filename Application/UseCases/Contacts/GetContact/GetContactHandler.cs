using Application.Common;
using Application.DTOs.Contact;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Contacts.GetContact;

public class GetContactHandler:IRequestHandler<GetContactQuery,Result<ReadContactDto>>
{
    private readonly IUserContactRepository _userContactRepository;
    private readonly IMapper _mapper;

    public GetContactHandler(IUserContactRepository userContactRepository, IMapper mapper)
    {
        _userContactRepository = userContactRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReadContactDto>> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        var userContact=await _userContactRepository.GetUserContactAsync(request.UserId,request.ContactId,
            cancellationToken,
            include=>include.Include(x=>x.Contact));
        return userContact is null ?
            Result<ReadContactDto>.Failure("User contact not found") : 
            Result<ReadContactDto>.Success(_mapper.Map<ReadContactDto>(userContact));
    }
}