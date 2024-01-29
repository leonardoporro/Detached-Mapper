using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features.Keys
{
    public class ForeignKeyTests
    {
        [Fact]
        public async Task fk_to_entity()
        {
            var dbContext = await TestDbContext.Create<KeyToEntityDbContext>();

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

        [Fact]
        public async Task fullname_fk_to_entity()
        {
            var dbContext = await TestDbContext.Create<KeyToEntityDbContext>();

            dbContext.FullKeyNameChildren.Add(new FullKeyNameChild { Name = "Child 1" });
            dbContext.FullKeyNameChildren.Add(new FullKeyNameChild { Name = "Child 2" });

            await dbContext.SaveChangesAsync();

            var result = dbContext.Map<FullKeyNameParent>(new
            {
                FullKeyNameParentId = 1,
                Name = "test user",
                FullKeyNameChildId = 2,
                FullKeyNameChildIds = new[] { 1, 2 }
            });

            Assert.NotNull(result.FullKeyNameChild);
            Assert.Equal(2, result.FullKeyNameChild.FullKeyNameChildId);
            Assert.Equal("Child 2", result.FullKeyNameChild.Name);

            Assert.NotNull(result.FullKeyNameChildren);
            Assert.Equal(1, result.FullKeyNameChildren[0].FullKeyNameChildId);
            Assert.Equal("Child 1", result.FullKeyNameChildren[0].Name);

            Assert.Equal(2, result.FullKeyNameChildren[1].FullKeyNameChildId);
            Assert.Equal("Child 2", result.FullKeyNameChildren[1].Name);
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

        public class FullKeyNameParent
        {
            public int FullKeyNameParentId { get; set; }

            public string Name { get; set; }

            public FullKeyNameChild FullKeyNameChild { get; set; }

            public List<FullKeyNameChild> FullKeyNameChildren { get; set; }
        }

        public class FullKeyNameChild
        {
            public int FullKeyNameChildId { get; set; }

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

            public DbSet<FullKeyNameParent> FullKeyNameParents { get; set; }

            public DbSet<FullKeyNameChild> FullKeyNameChildren { get; set; }
        }
    }
}