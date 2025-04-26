using Domain.Entities;
using Domain.Enums;
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

    public async Task AddAsync(UserChatEntity userChat,CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(userChat,cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserChatEntity?> GetByUserAndChatAsync(Guid userId, Guid chatId
        ,CancellationToken cancellationToken)
    {
        var obj=await _dbSet.FirstOrDefaultAsync(
            el=>el.UserId == userId && el.ChatId == chatId,
            cancellationToken);
        return obj;
    }

    public async Task<IEnumerable<UserChatEntity>> GetAdminsByChatIdAsync(Guid chatId
        ,CancellationToken cancellationToken)
    {
        var objs=await _dbSet.Where(x=>x.ChatId == chatId 
                                       && x.ChatRole==ChatRole.Admin)
            .ToListAsync(cancellationToken);
        return objs;
    }

    public async Task<IEnumerable<UserChatEntity>> GetChatsByUserIdAsync(Guid userId, bool includeChat,
        CancellationToken cancellationToken)
    {
        var query=_dbSet.Where(x=>x.UserId == userId);
        if (includeChat)
            query=query.Include(x=>x.Chat);
        var objs=await query.ToListAsync(cancellationToken);
        return objs;
    }
}