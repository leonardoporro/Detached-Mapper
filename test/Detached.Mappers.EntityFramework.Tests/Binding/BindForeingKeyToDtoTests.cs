using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Binding
{
    public class BindForeignKeyToDto
    {
        [Fact]
        public async Task bind_fk_to_dto()
        {
            var dbContext = await TestDbContext.Create<BindFkToDtoDbContext>();

            dbContext.Parents.Add(new ParentEntity
            {
                Id = 1,
                Name = "Parent 1",
                Child = new ChildEntity
                {
                    Id = 1,
                    Name = "Child 1"
                },
                Children = new List<ChildEntity>
                {
                    new ChildEntity { Id = 2, Name = "Child 2"},
                    new ChildEntity { Id = 3, Name = "Child 3"}
                }
            });

            await dbContext.SaveChangesAsync();

            var result = dbContext
                            .Project<ParentEntity, ParentDto>(dbContext.Parents)
                            .FirstOrDefault();

            Assert.Equal(1, result.ChildId);
            Assert.Equal(2, result.ChildIds[0]);
            Assert.Equal(3, result.ChildIds[1]);
        }

        public class ParentDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public int ChildId { get; set; }

            public List<int> ChildIds { get; set; }
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

        public class BindFkToDtoDbContext : TestDbContext
        {
            public BindFkToDtoDbContext(DbContextOptions<BindFkToDtoDbContext> options)
                : base(options)
            {
            }

            public DbSet<ParentEntity> Parents { get; set; }

            public DbSet<ChildEntity> Children { get; set; }
        }
    }
}