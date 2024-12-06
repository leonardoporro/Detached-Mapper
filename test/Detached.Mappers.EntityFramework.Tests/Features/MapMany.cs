using Detached.Annotations;
using Detached.Mappers;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Detached.Mappers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests.Features
{
    public class MapMany
    {
        [Fact]
        public async Task map_many()
        {
            var dbContext = await TestDbContext.Create<EntityTestDbContext>();

            dbContext.Users.Add(new User { Id = 1, Name = "usr1" });
            dbContext.Users.Add(new User { Id = 2, Name = "usr2" });

            await dbContext.SaveChangesAsync();

            var entities = await dbContext.MapAsync<User>(
                new[] {
                    new UserDto
                    {
                        Id = 1,
                        Name = "usr1_modified"
                    },
                    new UserDto
                    {
                        Id = 2,
                        Name = "usr2_modified"
                    }
                });

            await dbContext.SaveChangesAsync();

            Assert.Contains(entities, e => e.Name == "usr1_modified");
            Assert.Contains(entities, e => e.Name == "usr2_modified");

            var persisted = await dbContext.Users.ToListAsync();

            Assert.Contains(persisted, e => e.Name == "usr1_modified");
            Assert.Contains(persisted, e => e.Name == "usr2_modified");
        }

        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime DateOfBirth { get; set; }
        }

        public class UserDto
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime DateOfBirth { get; set; }
        }

        public class EntityTestDbContext : TestDbContext
        {
            public EntityTestDbContext(DbContextOptions<EntityTestDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            }
        }
    }
}