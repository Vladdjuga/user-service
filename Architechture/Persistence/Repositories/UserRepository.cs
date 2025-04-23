using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly MessengerDbContext _dbContext;
    private readonly DbSet<UserEntity> _dbSet;
    public UserRepository(MessengerDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<UserEntity>();
    }
    public async Task<IEnumerable<UserEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id,cancellationToken);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEntity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(o=>o.Email.Equals(email)
            ,cancellationToken);
        return userEntity;
    }

    public async Task<UserEntity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        var userEntity= await _dbSet.AsNoTracking().FirstOrDefaultAsync(o=>o.Username.Equals(userName)
            ,cancellationToken);
        return userEntity;
    }

    public async Task AddAsync(UserEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity,cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id,cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken)
    {
        var existingEntity = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
        if (existingEntity == null)
            throw new Exception("User not found");
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        _dbContext.Entry(existingEntity).Property(x => x.Id).IsModified = false;
        _dbContext.Entry(existingEntity).Property(x => x.CreatedAt).IsModified = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}