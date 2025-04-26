using Domain.Enums;

namespace Domain.Entities;

public class UserContactEntity
{
    public Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    public virtual UserEntity? User { get; init; }
    
    public required Guid ContactId { get; init; }
    public virtual UserEntity? Contact { get; init; }
    
    public required ContactStatus ContactStatus { get; set; }
    public required DateTime CreatedAt { get; init; }
    
    public required Guid PrivateChatId { get; init; }
    public virtual ChatEntity? PrivateChat { get; init; }
}