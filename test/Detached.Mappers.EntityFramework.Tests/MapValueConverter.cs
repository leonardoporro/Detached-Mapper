using Detached.Mappers.EntityFramework.Tests.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapValueConverter
    {
        [Fact]
        public async Task map_property_with_value_converter()
        {
            using var db = await DefaultTestDbContext.CreateAsync();

            var address = await db.MapAsync<Address>(new Address
            {
                Street = "Main St.",
                Number = "123",
                Tags = new List<Tag>
                 {
                     new Tag { Name = "primary" },
                     new Tag { Name = "local" }
                 }
            });

            await db.SaveChangesAsync();


            await db.MapAsync<Address>(new Address
            {
                Id = address.Id,
                Street = "Main St.",
                Number = "1234",
                Tags = new List<Tag>
                 {
                     new Tag { Name = "primary" },
                     new Tag { Name = "external" }
                 }
            });

            var persisted = db.Set<Address>().FirstOrDefault(a => a.Id == address.Id);

            Assert.Equal("Main St.", persisted.Street);
            Assert.Equal("1234", persisted.Number);
            Assert.Contains(persisted.Tags, t => t.Name == "primary");
            Assert.Contains(persisted.Tags, t => t.Name == "external");
        }
    }
}