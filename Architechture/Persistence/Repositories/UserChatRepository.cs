using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserChatRepository:IUserChatRepository
{
    private readonly MessengerDbContext _dbContext;
    private readonly DbSet<UserChatEntity> _dbSet;
    public UserChatRepository(MessengerDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<UserChatEntity>();
    }

    public async Task AddAsync(UserChatEntity userChat)
    {
        await _dbSet.AddAsync(userChat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserChatEntity?> GetByUserAndChatAsync(Guid userId, Guid chatId)
    {
        var obj=await _dbSet.FindAsync(userId,chatId);
        return obj;
    }

    public Task<IEnumerable<UserChatEntity>> GetAdminsByChatIdAsync(Guid chatId)
    {
        var objs=_dbSet.Where(x=>x.ChatId == chatId).ToList();
        return Task.FromResult<IEnumerable<UserChatEntity>>(objs);
    }
}