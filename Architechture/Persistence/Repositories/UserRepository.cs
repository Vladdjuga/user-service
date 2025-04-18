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
    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }
    public async Task<UserEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }
    public async Task AddAsync(UserEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task UpdateAsync(UserEntity entity)
    {
        var trackedEntity = await _dbContext.Users
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (trackedEntity is null)
            throw new Exception("User not found");
        var entry = _dbContext.Entry(entity);
        foreach (var p in entry.Properties)
        {
            var propertyName = p.Metadata.Name;
            if (propertyName is nameof(UserEntity.Id) or nameof(UserEntity.CreatedAt))
                    continue;
            var currValue=p.CurrentValue;
            var originalValue = _dbContext.Entry(trackedEntity)
                .Property(propertyName).CurrentValue;
            p.IsModified = !Equals(currValue, originalValue);
        }
        await _dbContext.SaveChangesAsync();
    }
}