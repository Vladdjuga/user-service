using Application.Common;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.Contacts.ChangeContactStatus;

public record ChangeContactStatusCommand(Guid UserId,Guid ContactId,ContactStatus ContactStatus) : IRequest<IResult>;