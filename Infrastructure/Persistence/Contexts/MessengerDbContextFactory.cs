using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence.Contexts;

public class MessengerDbContextFactory : IDesignTimeDbContextFactory<MessengerDbContext>
{
    public MessengerDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=localhost;Port=5432;Database=UserDB;Username=postgres;Password=qwe;";
        var optionsBuilder = new DbContextOptionsBuilder<MessengerDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseLazyLoadingProxies();

        return new MessengerDbContext(optionsBuilder.Options);
    }
}