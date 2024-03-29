using System.Diagnostics;
using Detached.Mappers.EntityFramework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Detached.Mappers.Tests.Contrib.ScottSoftware.Model;

public class TellDbContext : DbContext
{
    static SqliteConnection _connection;

    public DbSet<Creator> Creators { get; set; }

    public DbSet<Work> Works { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_connection == null)
        {
            _connection = new SqliteConnection($"DataSource=file:Tell.sqlite3?mode=memory&cache=shared");
            _connection.Open();
        }


        optionsBuilder
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .LogTo(message => Debug.WriteLine(message), LogLevel.Information)
            .EnableDetailedErrors()
            .UseMapping(builder =>
            {
                builder.Type<Work>()
                    .Member(f => f.Creator)
                    .Parent();

                builder.Type<Creator>()
                    .Member(c => c.Works)
                    .Composition();
            });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CreatorConfiguration());
        modelBuilder.ApplyConfiguration(new WorkConfiguration());
        modelBuilder.Entity<Creator>().HasMany(c => c.Works).WithOne(w => w.Creator);

        modelBuilder.Entity<Creator>().HasData(new Creator
        {
            Id = 1,
            FullName = @"Douglas Adams",
            PrimaryLanguage = "EN",
            Born = new DateTime(1952, 4, 11),
            Died = new DateTime(2001, 5, 11)
        });

        modelBuilder.Entity<Work>().HasData(new
        {
            Id = 1,
            CreatorId = 1,
            Title = @"Young Zaphod Plays It Safe",
            Language = "EN"
        });

        base.OnModelCreating(modelBuilder);
    }
}