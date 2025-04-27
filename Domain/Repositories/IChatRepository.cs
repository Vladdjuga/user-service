using Domain.Entities;

namespace Domain.Repositories;

public interface IChatRepository
{
    Task<IEnumerable<ChatEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<ChatEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> AnyByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(ChatEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(ChatEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}