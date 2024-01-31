using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class MapForeignKeyToEntity
    {
        [Fact]
        public async Task map_fk_to_entity()
        {
            var dbContext = await TestDbContext.Create<MapForeignKeyToEntityDbContext>();

            dbContext.Children.Add(new ChildEntity { Name = "Child 1" });
            dbContext.Children.Add(new ChildEntity { Name = "Child 2" });

            await dbContext.SaveChangesAsync();

            var result = dbContext.Map<ParentEntity>(new
            {
                Id = 1,
                Name = "test user",
                ChildId = 2,
                ChildIds = new[] { 1, 2 }
            });

            Assert.NotNull(result.Child);
            Assert.Equal(2, result.Child.Id);
            Assert.Equal("Child 2", result.Child.Name);

            Assert.NotNull(result.Children);
            Assert.Equal(1, result.Children[0].Id);
            Assert.Equal("Child 1", result.Children[0].Name);

            Assert.Equal(2, result.Children[1].Id);
            Assert.Equal("Child 2", result.Children[1].Name);
        }

        public class ParentEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ChildEntity Child { get; set; }

            public List<ChildEntity> Children { get; set; }
        }

        public class ChildEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class MapForeignKeyToEntityDbContext : TestDbContext
        {
            public MapForeignKeyToEntityDbContext(DbContextOptions<MapForeignKeyToEntityDbContext> options)
                : base(options)
            {
            }

            public DbSet<ParentEntity> Parents { get; set; }

            public DbSet<ChildEntity> Children { get; set; }
        }
    }
}