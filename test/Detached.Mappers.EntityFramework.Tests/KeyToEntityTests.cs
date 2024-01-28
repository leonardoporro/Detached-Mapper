using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class KeyToEntityTests
    {
        //[Fact]
        public async Task map_key_to_entity()
        {
            var dbContext = await TestDbContext.Create<KeyToEntityDbContext>();

            dbContext.Children.Add(new ChildEntity { Name = "Child 1" });
            dbContext.Children.Add(new ChildEntity { Name = "Child 2" });

            await dbContext.SaveChangesAsync();

            var result = dbContext.Map<ParentEntity>(new
            {
                Id = 1,
                Name = "test user",
                ChildId = 2
            });

            Assert.NotNull(result.Child);
            Assert.Equal(2, result.Child.Id);
            Assert.Equal("Child 2", result.Child.Name);
        }

        public class ParentEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ChildEntity Child { get; set; }
        }

        public class ChildEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class KeyToEntityDbContext : TestDbContext
        {
            public KeyToEntityDbContext(DbContextOptions<KeyToEntityDbContext> options)
                : base(options)
            {
            }

            public DbSet<ParentEntity> Parents { get; set; }

            public DbSet<ChildEntity> Children { get; set; }
        }
    }
}