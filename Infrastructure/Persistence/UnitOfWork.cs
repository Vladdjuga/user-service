using Application;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence;

public class UnitOfWork: IUnitOfWork
{
    private readonly MessengerDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IUserChatRepository _userChatRepository;
    public UnitOfWork(MessengerDbContext dbContext, IUserRepository userRepository
    , IChatRepository chatRepository, IUserChatRepository userChatRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _userChatRepository = userChatRepository;
    }
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}