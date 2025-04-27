using Application.Common;
using Application.DTOs.Contact;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Contacts.GetUsersContacts;

public class GetUsersContactsHandler:IRequestHandler<GetUsersContactsQuery,Result<IEnumerable<ReadContactDto>>>
{
    private readonly IUserContactRepository _userContactRepository;
    private readonly IMapper _mapper;

    public GetUsersContactsHandler(IUserContactRepository userContactRepository, IMapper mapper)
    {
        _userContactRepository = userContactRepository;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<ReadContactDto>>> Handle(GetUsersContactsQuery request, CancellationToken cancellationToken)
    {
        var userContacts=await _userContactRepository
            .GetAllUsersContactsAsync(request.UserId,cancellationToken);
        return !userContacts.Any() ?
            Result<IEnumerable<ReadContactDto>>.Failure("User contact not found") : 
            Result<IEnumerable<ReadContactDto>>.Success(_mapper.Map<IEnumerable<ReadContactDto>>(userContacts));
    }
}