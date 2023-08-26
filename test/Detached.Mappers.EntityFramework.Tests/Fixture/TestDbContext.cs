using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Tests.Fixture
{
    public class TestDbContext : DbContext
    {
        protected TestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected virtual void ConfigureMapping(EFMapperConfigurationBuilder builder)
        {

        }

        public static async Task<TDbContext> Create<TDbContext>([CallerMemberName]string dbName = null)
            where TDbContext : TestDbContext
        {
            var connection = new SqliteConnection($"DataSource=file:{Guid.NewGuid()}.db");

            connection.Open();

            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();

            DbContextInstance dbContextInstance = new DbContextInstance();

            optionsBuilder
                     .UseSqlite(connection)
                     .EnableSensitiveDataLogging()
                     .EnableDetailedErrors()
                     .UseMapping(builder =>
                     {
                         dbContextInstance.DbContext.ConfigureMapping(builder);
                     });

            
            var dbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), new object[] { optionsBuilder.Options });

            await dbContext.Database.EnsureCreatedAsync();

            dbContextInstance.DbContext = dbContext;

            return dbContext;
        }

        class DbContextInstance
        {
            public TestDbContext DbContext { get; set; }
        }
    }
}
