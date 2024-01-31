using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Detached.Mappers.Types;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class ConfigureTypeFluent
    {
        [Fact]
        public async Task apply_conventions_to_fluent()
        {
            var dbContext = await TestDbContext.Create<FluentTestDbContext>();

            IType typeOptions = dbContext.GetMapper().Options.GetType(typeof(ConfiguredTestClass));

            Assert.True(typeOptions.IsEntity());
            Assert.True(typeOptions.GetMember(nameof(ConfiguredTestClass.CustomizedKey1)).IsKey());
            Assert.True(typeOptions.GetMember(nameof(ConfiguredTestClass.CustomizedKey2)).IsKey());
            Assert.False(typeOptions.GetMember(nameof(ConfiguredTestClass.Id)).IsKey());
        }

        public class ConfiguredTestClass
        {
            public int Id { get; set; }

            public int CustomizedKey1 { get; set; }

            public int CustomizedKey2 { get; set; }

            public string Name { get; set; }
        }

        public class FluentTestDbContext : TestDbContext
        {
            public FluentTestDbContext(DbContextOptions<FluentTestDbContext> options)
                : base(options)
            {
            }

            public override void OnMapperCreating(EntityMapperOptions builder)
            {
                builder.Default(mapperOptions =>
                {
                    mapperOptions.Type<ConfiguredTestClass>().Type.Entity(true);
                    mapperOptions.Type<ConfiguredTestClass>().Member(c => c.CustomizedKey1).Key(true);
                    mapperOptions.Type<ConfiguredTestClass>().Member(c => c.CustomizedKey2).Key(true);
                    mapperOptions.Type<ConfiguredTestClass>().Key(c => c.CustomizedKey1, c => c.CustomizedKey2);
                });
            }

            protected override void OnModelCreating(ModelBuilder mb)
            {
                mb.Entity<ConfiguredTestClass>().HasKey(c => new { c.CustomizedKey1, c.CustomizedKey2 });
            }
        }
    }
}