using Detached.Mappers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Tests.Fixture
{
    public class TestDbContext : DbContext
    {  
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual void OnMapperCreating(EntityMapperOptionsBuilder builder)
        {

        } 

        public static async Task<TDbContext> Create<TDbContext>(bool overwrite = true,  [CallerMemberName]string dbName = null)
            where TDbContext : TestDbContext
        {
            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<TDbContext>(opts =>
            {
                 opts.UseSqlite($"DataSource=file:{dbName}?mode=memory&cache=shared")
                     //.UseSqlServer($"Server=localhost\\SQLEXPRESS;Database={dbName};User Id=sa;Password=Sa12345.;Encrypt=False;")
                     .EnableSensitiveDataLogging()
                     .EnableDetailedErrors()
                     .UseMapping();
            });

            var serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetRequiredService<TDbContext>();

            if (overwrite)
            {
                await dbContext.Database.EnsureDeletedAsync();
            }

            await dbContext.Database.EnsureCreatedAsync(); 

            return dbContext;
        } 
    }
}