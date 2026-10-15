using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.DTOs
{
    // https://github.com/leonardoporro/Detached-Mapper/issues/120
    // https://github.com/leonardoporro/Detached-Mapper/issues/126
    public class DateOnlyEntityTests
    {
        [Fact]
        public async Task map_dto_with_dateonly_to_new_entity()
        {
            var dbContext = await TestDbContext.Create<DateOnlyTestDbContext>();

            Category category = await dbContext.MapAsync<Category>(new CategorySaveVM
            {
                Id = 1,
                Name = "category",
                CreatedAt = new DateOnly(2025, 11, 12),
                UpdatedAt = null,
                OpensAt = new TimeOnly(9, 0)
            });

            await dbContext.SaveChangesAsync();

            dbContext.ChangeTracker.Clear();

            Category saved = await dbContext.Categories.FirstAsync(c => c.Id == 1);
            Assert.Equal("category", saved.Name);
            Assert.Equal(new DateOnly(2025, 11, 12), saved.CreatedAt);
            Assert.Null(saved.UpdatedAt);
            Assert.Equal(new TimeOnly(9, 0), saved.OpensAt);
        }

        [Fact]
        public async Task map_dto_with_dateonly_to_existing_entity()
        {
            var dbContext = await TestDbContext.Create<DateOnlyTestDbContext>();

            dbContext.Categories.Add(new Category { Id = 1, Name = "old", CreatedAt = new DateOnly(2024, 1, 1) });
            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();

            await dbContext.MapAsync<Category>(new CategorySaveVM
            {
                Id = 1,
                Name = "new",
                CreatedAt = new DateOnly(2024, 1, 1),
                UpdatedAt = new DateOnly(2025, 11, 12)
            });

            await dbContext.SaveChangesAsync();
            dbContext.ChangeTracker.Clear();

            Category saved = await dbContext.Categories.FirstAsync(c => c.Id == 1);
            Assert.Equal("new", saved.Name);
            Assert.Equal(new DateOnly(2025, 11, 12), saved.UpdatedAt);
        }

        public class Category
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }

            [MaxLength(50)]
            public string Name { get; set; }

            public bool IsActive { get; set; } = true;

            public DateOnly CreatedAt { get; set; }

            public DateOnly? UpdatedAt { get; set; }

            public TimeOnly? OpensAt { get; set; }
        }

        public class CategorySaveVM
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateOnly CreatedAt { get; set; }

            public DateOnly? UpdatedAt { get; set; }

            public TimeOnly? OpensAt { get; set; }
        }

        public class DateOnlyTestDbContext : TestDbContext
        {
            public DateOnlyTestDbContext(DbContextOptions<DateOnlyTestDbContext> options)
                : base(options)
            {
            }

            public DbSet<Category> Categories { get; set; }
        }
    }
}
