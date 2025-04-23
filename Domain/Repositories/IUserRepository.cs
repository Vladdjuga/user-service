using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserEntity?> GetByIdAsync(Guid id,CancellationToken cancellationToken);
    Task<UserEntity?> GetByEmailAsync(string email,CancellationToken cancellationToken);
    Task<UserEntity?> GetByUserNameAsync(string userName,CancellationToken cancellationToken);
    Task AddAsync(UserEntity entity,CancellationToken cancellationToken);
    Task UpdateAsync(UserEntity entity,CancellationToken cancellationToken);
    Task DeleteAsync(Guid id,CancellationToken cancellationToken);
}