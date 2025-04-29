using System.IdentityModel.Tokens.Jwt;
using Application.DTOs.Contact;
using Application.UseCases.Contacts.ChangeContactStatus;
using Application.UseCases.Contacts.CreateContact;
using Application.UseCases.Contacts.GetContact;
using Application.UseCases.Contacts.GetUsersContacts;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Data;

[Route("api/[controller]")]
public class ContactController:Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ContactController(IMediator mediator, ILogger<ContactController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("addContact/{contactId:guid}")]
    public async Task<Results<Ok<ReadContactDto>,BadRequest<string>>> CreateContactById(Guid contactId)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            _logger.LogInformation("User {Username} not found", userGuid);
            return TypedResults.BadRequest("User not found");
        }
        
        var command = new CreateContactCommand(
            userGuid,
            contactId
        );
        _logger.LogInformation("Creating contact {ContactId}", contactId);
        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogInformation("Failed to create contact for {UserId} and {ContactId}",
                userGuid, contactId);
            _logger.LogInformation("Error: {Error}", result.Error);
            return TypedResults.BadRequest(result.Error);
        }
        _logger.LogInformation("Created contact {ContactId}", contactId);
        return TypedResults.Ok(result.Value);
    }
    [Authorize]
    [HttpGet("getContact/{contactId:guid}")]
    public async Task<Results<Ok<ReadContactDto>,BadRequest<string>>> GetContactById(Guid contactId)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            _logger.LogInformation("User {Username} not found", userGuid);
            return TypedResults.BadRequest("User not found");
        }
        var query=new GetContactQuery(userGuid,contactId);
        _logger.LogInformation("Getting contact {ContactId}", contactId);
        var result = await _mediator.Send(query);
        if (result.IsFailure)
        {
            _logger.LogError("Error getting contact {ContactId}", contactId);
            _logger.LogInformation("Error: {Error}", result.Error);
            return TypedResults.BadRequest(result.Error);
        }
        _logger.LogInformation("Retrieved contact {ContactId}", contactId);
        return TypedResults.Ok(result.Value);
    }
    [Authorize]
    [HttpGet("getContacts")]
    public async Task<Results<Ok<IEnumerable<ReadContactDto>>, BadRequest<string>>> GetContacts()
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            _logger.LogInformation("User {Username} not found", userGuid);
            return TypedResults.BadRequest("User not found");
        }

        var query = new GetUsersContactsQuery(userGuid);
        _logger.LogInformation("Getting contacts for {UserId}", userGuid);
        var result = await _mediator.Send(query);
        if (result.IsFailure)
        {
            _logger.LogError("Error getting contacts for {UserId}", userGuid);
            _logger.LogInformation("Error: {Error}", result.Error);
            return TypedResults.BadRequest(result.Error);
        }
        _logger.LogInformation("Retrieved contacts for {UserId}", userGuid);
        return TypedResults.Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("changeContactStatus{contactId:guid}/{status}")]
    public async Task<Results<Ok, BadRequest<string>>> ChangeContactStatus(Guid contactId,
        ContactStatus status)
    {
        var userId=User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (!Guid.TryParse(userId, out Guid userGuid))
        {
            _logger.LogInformation("User {Username} not found", userGuid);
            return TypedResults.BadRequest("User not found");
        }

        var command = new ChangeContactStatusCommand(
            userGuid,
            contactId,
            status
        );
        _logger.LogInformation("Changing contact status to {Status}", status);
        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogError("Error changing status for contact : {ContactId}", contactId);
            _logger.LogInformation("Error: {Error}", result.Error);
            return TypedResults.BadRequest(result.Error);
        }
        _logger.LogInformation("Successfully changed contact status to {Status}", status);
        return TypedResults.Ok();
    }
}