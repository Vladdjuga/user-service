using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task<UserEntity?> GetByIdAsync(Guid id);
    Task AddAsync(UserEntity entity);
    Task UpdateAsync(UserEntity entity);
    Task DeleteAsync(Guid id);
}