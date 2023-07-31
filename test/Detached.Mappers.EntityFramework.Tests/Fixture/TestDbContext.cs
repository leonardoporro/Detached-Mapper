using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Detached.Mappers.EntityFramework.Tests.Fixture
{
    public abstract class TestDbContext : DbContext
    {
        public TestDbContext()
            : base(CreateOptions())
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite().UseMapping();
        }

        protected virtual void OnMapperCreating(EFMapperConfigurationBuilder builder)
        {
        }

        static DbContextOptions CreateOptions()
        {
            var connection = new SqliteConnection($"DataSource=file:{Guid.NewGuid()}.db");

            connection.Open();

            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder
                     .UseSqlite(connection)
                     .EnableSensitiveDataLogging()
                     .EnableDetailedErrors()
                     .UseMapping();

            return optionsBuilder.Options;
        }
    }
}
