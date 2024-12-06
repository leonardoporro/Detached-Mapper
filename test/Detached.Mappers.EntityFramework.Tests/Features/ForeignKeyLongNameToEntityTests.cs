using Detached.Mappers.EntityFramework;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class ForeignKeyLongNameToEntityTests
    {
        [Fact]
        public async Task map_fk_longname_to_entity()
        {
            var dbContext = await TestDbContext.Create<MapForeignKeyLongNameToEntityDbContext>();

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
            Assert.Equal(2, result.Child.ChildId);
            Assert.Equal("Child 2", result.Child.Name);

            Assert.NotNull(result.Children);
            Assert.Equal(1, result.Children[0].ChildId);
            Assert.Equal("Child 1", result.Children[0].Name);

            Assert.Equal(2, result.Children[1].ChildId);
            Assert.Equal("Child 2", result.Children[1].Name);
        }

        public class ParentEntity
        {
            [Key]
            public int ParentId { get; set; }

            public string Name { get; set; }

            public ChildEntity Child { get; set; }

            public List<ChildEntity> Children { get; set; }
        }

        public class ChildEntity
        {
            [Key]
            public int ChildId { get; set; }

            public string Name { get; set; }
        }

        public class MapForeignKeyLongNameToEntityDbContext : TestDbContext
        {
            public MapForeignKeyLongNameToEntityDbContext(DbContextOptions<MapForeignKeyLongNameToEntityDbContext> options)
                : base(options)
            {
            }

            public DbSet<ParentEntity> Parents { get; set; }

            public DbSet<ChildEntity> Children { get; set; }
        }
    }
}