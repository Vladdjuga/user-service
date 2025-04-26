using Domain.Enums;

namespace Application.DTOs.Chat;

public class ReadChatDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required bool IsPrivate { get; init; }
    public required ChatType ChatType { get; init; } 
    public required bool IsMuted { get; init; }
    public required ChatRole ChatRole { get; init; }
    public required DateTime CreatedAt { get; init; }
    
}