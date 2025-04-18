using Domain.Enums;

namespace Domain.Entities;

public class ChatEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required bool IsPrivate { get; set; }
    public required ChatType ChatType { get; set; } 
    public required DateTime CreatedAt { get; set; }
    public virtual IEnumerable<UserChatEntity> UserChatEntities { get; set; } = new List<UserChatEntity>();
}