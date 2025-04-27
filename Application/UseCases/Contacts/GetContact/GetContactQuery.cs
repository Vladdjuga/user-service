using Application.Common;
using Application.DTOs.Contact;
using MediatR;

namespace Application.UseCases.Contacts.GetContact;

public record GetContactQuery(Guid UserId,Guid ContactId):IRequest<Result<ReadContactDto>>;