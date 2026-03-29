using Microsoft.EntityFrameworkCore;
using server.models;


namespace server.database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; } = null!;
    }
}