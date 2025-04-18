using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ChatRepository:IChatRepository
{
    private readonly MessengerDbContext _dbContext;
    private readonly DbSet<ChatEntity> _dbSet;
    public ChatRepository(MessengerDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<ChatEntity>();
    }
    public async Task<IEnumerable<ChatEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<ChatEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(ChatEntity entity)
    {
        await _dbSet.AddAsync(entity); 
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ChatEntity entity)
    {
        var trackedEntity = await _dbContext.Chats
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);
        if (trackedEntity is null)
            throw new Exception("Chat not found");
        var entry = _dbContext.Entry(entity);
        foreach (var p in entry.Properties)
        {
            var propertyName = p.Metadata.Name;
            if (propertyName is nameof(ChatEntity.Id) or nameof(ChatEntity.CreatedAt))
                continue;
            var currValue=p.CurrentValue;
            var originalValue = _dbContext.Entry(trackedEntity)
                .Property(propertyName).CurrentValue;
            p.IsModified = !Equals(currValue, originalValue);
        }
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
}