using Domain.Enums;

namespace Application.DTOs.Chat;

public class AddUserToChatDto
{
    public required Guid ChatId;
    public required Guid UserId;
    public required ChatRole ChatRole;
}