using Domain.Enums;

namespace Domain.Entities;

public class UserChatEntity
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }
    
    public virtual UserEntity? User { get; init; }
    public virtual ChatEntity? Chat { get; init; }
    
    public required bool IsMuted { get; init; }
    public required ChatRole ChatRole { get; init; }
}