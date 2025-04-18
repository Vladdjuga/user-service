using Domain.Entities;

namespace Domain.Repositories;

public interface IUserChatRepository
{
    Task AddAsync(UserChatEntity userChat);
    Task<UserChatEntity?> GetByUserAndChatAsync(Guid userId, Guid chatId);
    Task<IEnumerable<UserChatEntity>> GetAdminsByChatIdAsync(Guid chatId);
}