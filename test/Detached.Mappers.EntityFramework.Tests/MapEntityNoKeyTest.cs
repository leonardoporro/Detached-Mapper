using Detached.Mappers.EntityFramework.Tests.Fixture;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapEntityNoKeyTest
    {
        [Fact]
        public async Task map_entity_nokey_dto()
        {
            var dbContext = await TestDbContext.Create<KeylessTestDbContext>();

            dbContext.Roles.Add(new Role { Id = 1, Name = "admin" });
            dbContext.Roles.Add(new Role { Id = 2, Name = "user" });
            await dbContext.SaveChangesAsync();

            User user = await dbContext.MapAsync<User>(new NoKeyUserDTO
            {
                Name = "nokeyuser",
                Roles = new List<Role>
                {
                    new Role { Id = 1 },
                    new Role { Id = 2 }
                }
            });

            await dbContext.SaveChangesAsync();

            User persisted = dbContext.Users.Include(u => u.Roles)
                    .Where(u => u.Name == "nokeyuser")
                    .FirstOrDefault();

            Assert.NotNull(persisted);
            Assert.Equal("nokeyuser", persisted.Name);
            Assert.NotNull(persisted.Roles);
        }

        public class NoKeyUserDTO
        {
            public string Name { get; set; }

            public virtual List<Role> Roles { get; set; }
        }

        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime DateOfBirth { get; set; }

            public virtual List<Role> Roles { get; set; }
        }

        public class Role
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class UserRole
        {
            public virtual int UserId { get; set; }

            public virtual User User { get; set; }

            public virtual int RoleId { get; set; }

            public virtual Role Role { get; set; }
        }

        public class KeylessTestDbContext : TestDbContext
        {
            public KeylessTestDbContext(DbContextOptions<KeylessTestDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }

            public DbSet<Role> Roles { get; set; }

            protected override void OnModelCreating(ModelBuilder mb)
            {
                mb.Entity<User>()
                   .HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<UserRole>(
                       ur => ur.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId),
                       ur => ur.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId))
                   .HasKey(ur => new { ur.UserId, ur.RoleId });
            }
        }
    }
}