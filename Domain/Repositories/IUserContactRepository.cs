using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;

public interface IUserContactRepository
{
    Task<IEnumerable<UserEntity?>> GetAllUsersContactsAsync(Guid id, CancellationToken cancellationToken);
    Task<UserContactEntity?> GetUserContactAsync(Guid userId, Guid contactId, CancellationToken cancellationToken);
    Task AddAsync(UserContactEntity userContactEntity, CancellationToken cancellationToken);
    Task UpdateAsync(UserContactEntity userContactEntity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task ChangeStatusAsync(Guid userId,Guid contactId, ContactStatus status, CancellationToken cancellationToken);
    Task ChangeStatusAsync(Guid userContactId, ContactStatus status, CancellationToken cancellationToken);
}