using Domain.Enums;

namespace Domain.Entities;

public class UserChatEntity
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    
    public virtual UserEntity? User { get; set; }
    public virtual ChatEntity? Chat { get; set; }
    
    public required bool IsMuted { get; set; }
    public required ChatRole ChatRole { get; set; }
}