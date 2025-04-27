using Application.Common;
using Application.DTOs.Contact;
using MediatR;

namespace Application.UseCases.Contacts.GetUsersContacts;

public record GetUsersContactsQuery(Guid UserId):IRequest<Result<IEnumerable<ReadContactDto>>>;