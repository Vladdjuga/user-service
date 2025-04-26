using Domain.Entities;

namespace Domain.Repositories;

public interface IUserChatRepository
{
    Task AddAsync(UserChatEntity userChat, CancellationToken cancellationToken);
    Task<UserChatEntity?> GetByUserAndChatAsync(Guid userId, Guid chatId, CancellationToken cancellationToken);
    Task<IEnumerable<UserChatEntity>> GetAdminsByChatIdAsync(Guid chatId, CancellationToken cancellationToken);
}