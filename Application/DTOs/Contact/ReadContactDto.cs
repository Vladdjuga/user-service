using Domain.Enums;

namespace Application.DTOs.Contact;

public class ReadContactDto
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required ContactStatus Status { get; set; }
    public required Guid PrivateChatId { get; init; }
    public required DateTime CreatedAt { get; init; }
}