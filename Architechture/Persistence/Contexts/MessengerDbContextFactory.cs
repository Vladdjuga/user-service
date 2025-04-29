using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence.Contexts;

public class MessengerDbContextFactory : IDesignTimeDbContextFactory<MessengerDbContext>
{
    public MessengerDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=UserDB;Trusted_connection=true;";
        var optionsBuilder = new DbContextOptionsBuilder<MessengerDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.UseLazyLoadingProxies();

        return new MessengerDbContext(optionsBuilder.Options);
    }
}