using Detached.Mappers.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class MappingStrategies
{
    public static async Task Run()
    {
        TestDbContext dbContext = new TestDbContext();
        await dbContext.Database.EnsureCreatedAsync();

     

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Mapping Strategies");
        Console.WriteLine("----------------------------------------------------------------------------------------");
        foreach (User persistedUser in dbContext.Users)
        {
            Console.WriteLine($"Id: {persistedUser.Id}, Name = '{persistedUser.Name}'");
        }
    }

    class TestDbContext : DbContext
    {
        static SqliteConnection _connection;

        static TestDbContext()
        {
            _connection = new SqliteConnection($"DataSource=file:{Guid.NewGuid()}?mode=memory&cache=shared");
            _connection.Open();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connection).UseMapping(cfg =>
            {
                cfg.Default(opts =>
                {
                    opts.Type<User>().Constructor(x => new User { Name = "new!" });
                });
            });
        }

        public DbSet<User> Users { get; set; }
    }

    class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}