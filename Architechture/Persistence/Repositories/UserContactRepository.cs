using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserContactRepository:IUserContactRepository
{
    private readonly MessengerDbContext _dbContext;
    private readonly DbSet<UserContactEntity> _dbSet;

    public UserContactRepository(MessengerDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<UserContactEntity>();
    }
    public async Task<IEnumerable<UserEntity?>> GetAllUsersContactsAsync(Guid id, CancellationToken cancellationToken)
    {
        var contacts = await _dbSet.Where(el => el.UserId == id)
            .Include(el=>el.Contact)
            .Select(el => el.Contact).ToListAsync(cancellationToken);
        return contacts;
    }

    public async Task<UserContactEntity?> GetUserContactAsync(Guid userId, Guid contactId, CancellationToken cancellationToken)
    {
        var userContacts= await _dbSet.Where(el => el.UserId == userId && el.ContactId == contactId)
            .Include(el => el.Contact)
            .Include(el => el.User)
            .FirstOrDefaultAsync(cancellationToken);
        return userContacts;
    }

    public async Task AddAsync(UserContactEntity userContactEntity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(userContactEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserContactEntity userContactEntity, CancellationToken cancellationToken)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(x=>x.Id == userContactEntity.Id, cancellationToken);
        if(existingEntity==null)
            throw new ApplicationException("User not found");
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(userContactEntity);
        _dbContext.Entry(existingEntity).Property(x=>x.CreatedAt).IsModified = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var ent = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new ApplicationException("Entity not found");
        _dbSet.Remove(ent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeStatusAsync(Guid userId, Guid contactId, ContactStatus status, CancellationToken cancellationToken)
    {
        var userContact= await _dbSet.FirstOrDefaultAsync(
            el=>el.UserId == userId && el.ContactId == contactId,
            cancellationToken)?? throw new ApplicationException("Entity not found");
        userContact.ContactStatus = status;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeStatusAsync(Guid userContactId, ContactStatus status, CancellationToken cancellationToken)
    {
        var userContact= await _dbSet.FindAsync([userContactId], cancellationToken)
            ?? throw new ApplicationException("Entity not found");
        userContact.ContactStatus = status;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}