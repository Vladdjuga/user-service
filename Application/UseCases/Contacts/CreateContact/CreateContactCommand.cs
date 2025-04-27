using Application.Common;
using Application.DTOs.Contact;
using MediatR;

namespace Application.UseCases.Contacts.CreateContact;

public record CreateContactCommand(Guid UserId,Guid ContactId):IRequest<Result<ReadContactDto>>;