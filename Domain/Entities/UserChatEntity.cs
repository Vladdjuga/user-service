using Domain.Enums;

namespace Domain.Entities;

public class UserChatEntity
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    
    public virtual required UserEntity User { get; set; }
    public virtual required ChatEntity Chat { get; set; }
    
    public required bool IsMuted { get; set; }
    public required ChatRole ChatRole { get; set; }
}