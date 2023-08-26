using Detached.Annotations;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class MapReadOnlyPropertyTests
    {
        [Fact]
        public async Task map_getter_only_collection()
        {
            var dbContext = await TestDbContext.Create<ReadOnlyPropertyTestDbContext>();

            var result = dbContext.Map<User>(new
            {
                Id = 1,
                Name = "test user",
                Addresses = new[]
                {
                    new { Id = 1, Line1 = "123 Main St." }
                }
            });

            Assert.Equal("test user", result.Name);
            Assert.Equal(1, result.Addresses[0].Id);
            Assert.Equal("123 Main St.", result.Addresses[0].Line1);
        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }

            [Composition]
            public List<Address> Addresses { get; } = new List<Address>();
        }

        public class Address
        {
            public int Id { get; set; }

            public string Line1 { get; set; }

            public string Line2 { get; set; }
        }

        public class ReadOnlyPropertyTestDbContext : TestDbContext
        {
            protected ReadOnlyPropertyTestDbContext(DbContextOptions options) 
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
        }
    }
}
