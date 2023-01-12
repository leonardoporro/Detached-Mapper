using Detached.Mappers.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Detached.Mappers;
public class GettingStarted
{
    public static async Task Run()
    {
        TestDbContext dbContext = new TestDbContext();
        await dbContext.Database.EnsureCreatedAsync();

        dbContext.Map<User>(new User { Id = 1, Name = "mapped from Entity" });
        dbContext.Map<User>(new UserDTO { Id = 2, Name = "mapped from DTO" });
        dbContext.Map<User>(new { Id = 3, Name = "mapped from Anonymous" });
        dbContext.Map<User>(new Dictionary<string, object> { { "Id", 4 }, { "Name", "mapped from Dictionary" } });

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Getting Started");
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


                    opts.Type<User>().Entity()
                        .Member(u => u.Id).Key()
                        
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