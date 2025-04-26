using Application.Common;
using MediatR;

namespace Application.UseCases.Contacts.CreateContact;

public record CreateContactCommand(Guid UserId,Guid ContactId):IRequest<Result<Guid>>;