using Domain.Enums;

namespace Application.DTOs.Chat;

public class CreateChatDto
{
    public required string Title { get; init; }
    public required bool IsPrivate { get; init; }
    public required ChatType ChatType { get; init; } 
}