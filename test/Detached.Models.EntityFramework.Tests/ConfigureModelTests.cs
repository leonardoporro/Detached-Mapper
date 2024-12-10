using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Detached.Models.EntityFramework.Tests
{
    public class ConfigureModelTests
    {
        [Fact]
        public void DbContext_ConfigureModel_Success()
        {
            var services = new ServiceCollection();

            services.AddDbContext<TestDbContext>((services, options) =>
            {
                options.UseSqlite($"DataSource=file:test_configure_model?mode=memory&cache=shared");
                options.UseModel(services);
            });

            services.ConfigureModel<TestDbContext>((model, dbContext) =>
            {
                model.Entity<Entity1>().HasKey(e => e.Id1);
            });

            services.ConfigureModel<TestDbContext>((model, dbContext) =>
            {
                model.Entity<Entity2>().HasKey(e => e.Id2);
            });

            var serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetRequiredService<TestDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.Set<Entity1>().Add(new Entity1 { Id1 = 1, Name = "Entity 1" });
            dbContext.Set<Entity2>().Add(new Entity2 { Id2 = 2, Name = "Entity 2" });

            dbContext.SaveChanges();

            var entity1 = dbContext.Find<Entity1>(1);
            var entity2 = dbContext.Find<Entity2>(2);

            Assert.NotNull(entity1);
            Assert.Equal("Entity 1", entity1.Name);

            Assert.NotNull(entity2);
            Assert.Equal("Entity 2", entity2.Name);
        }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) 
            : base(options)
        {
        }
    }

    public class Entity1
    {
        public int Id1 { get; set; }

        public string Name { get; set; }
    }

    public class Entity2
    {
        public int Id2 { get; set; }

        public string Name { get; set; }
    }
}