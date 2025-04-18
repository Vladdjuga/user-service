using Domain.Entities;

namespace Domain.Repositories;

public interface IChatRepository
{
    Task<IEnumerable<ChatEntity>> GetAllAsync();
    Task<ChatEntity?> GetByIdAsync(Guid id);
    Task AddAsync(ChatEntity entity);
    Task UpdateAsync(ChatEntity entity);
    Task DeleteAsync(Guid id);
}