using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Detached.Mappers.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Configuration
{
    public class ConfigureFromServicesTests
    {
        [Fact]
        public void configure_mapper_from_services()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ModuleTestDbContext>((services, db) =>
            {
                db.UseSqlite($"DataSource=file:{nameof(configure_mapper_from_services)}?mode=memory&cache=shared");
                db.UseMapping(services);
            });


            services.ConfigureMapper<ModuleTestDbContext>(builder =>
            {
                builder.Type<ModuleTestClass>().Entity(true);
                builder.Type<ModuleTestClass>().Member(c => c.CustomizedKey1).Key(true);
                builder.Type<ModuleTestClass>().Member(c => c.CustomizedKey2).Key(true);
                builder.Type<ModuleTestClass>().Key(c => c.CustomizedKey1, c => c.CustomizedKey2);
            });
            
            var serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetRequiredService<ModuleTestDbContext>();

            IType typeOptions = dbContext.GetMapper().Options.GetType(typeof(ModuleTestClass));

            Assert.True(typeOptions.IsEntity());
            Assert.True(typeOptions.GetMember(nameof(ModuleTestClass.CustomizedKey1)).IsKey());
            Assert.True(typeOptions.GetMember(nameof(ModuleTestClass.CustomizedKey2)).IsKey());
            Assert.False(typeOptions.GetMember(nameof(ModuleTestClass.Id)).IsKey());
        }
    }

    public class ModuleTestClass
    {
        public int Id { get; set; }

        public int CustomizedKey1 { get; set; }

        public int CustomizedKey2 { get; set; }

        public string Name { get; set; }
    }

    public class ModuleTestDbContext : TestDbContext
    {
        public ModuleTestDbContext(DbContextOptions<ModuleTestDbContext> options)
            : base(options)
        {
        }
 
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<ModuleTestClass>().HasKey(c => new { c.CustomizedKey1, c.CustomizedKey2 });
        }
    }
}
