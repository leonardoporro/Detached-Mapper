using Detached.Mappers.Annotations;
using Detached.Mappers.EntityFramework.Tests.Model;
using Detached.Mappers.Types;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class FluentConfigurationTests
    {
        [Fact]
        public async Task apply_conventions_to_fluent()
        {
            DefaultTestDbContext context = new DefaultTestDbContext(await DefaultTestDbContext.CreateOptionsAsync("ConfigDb"));

            IType typeOptions = context.GetMapper().Options.GetType(typeof(ConfiguredTestClass));

            Assert.True(typeOptions.IsEntity());
            Assert.True(typeOptions.GetMember(nameof(ConfiguredTestClass.CustomizedKey1)).IsKey());
            Assert.True(typeOptions.GetMember(nameof(ConfiguredTestClass.CustomizedKey2)).IsKey());
            Assert.False(typeOptions.GetMember(nameof(ConfiguredTestClass.Id)).IsKey());
        }
    }

    public class ConfiguredTestClass
    {
        public int Id { get; set; }

        public int CustomizedKey1 { get; set; }

        public int CustomizedKey2 { get; set; }

        public string Name { get; set; }

        public static void Configure(MapperOptions mapperOptions)
        {
            mapperOptions.Type<ConfiguredTestClass>().TypeOptions.Entity(true);
            mapperOptions.Type<ConfiguredTestClass>().Member(c => c.CustomizedKey1).Key(true);
            mapperOptions.Type<ConfiguredTestClass>().Member(c => c.CustomizedKey2).Key(true);


            mapperOptions.Type<ConfiguredTestClass>().Key(c => c.CustomizedKey1, c => c.CustomizedKey2);
        }
    }
}