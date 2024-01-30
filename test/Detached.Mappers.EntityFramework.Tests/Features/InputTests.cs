using Detached.Annotations;
using Detached.Mappers;
using Detached.Mappers.EntityFramework.Extensions;
using Detached.Mappers.EntityFramework.Tests.Fixture;
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
    public class InputTests
    {
        [Fact]
        public async Task map_input()
        {
            var dbContext = await TestDbContext.Create<InputTestDbContext>();

            dbContext.Roles.Add(new Role { Id = 1, Name = "admin" });
            dbContext.Roles.Add(new Role { Id = 2, Name = "user" });
            dbContext.UserTypes.Add(new UserType { Id = 1, Name = "system" });
            await dbContext.SaveChangesAsync();

            User user = new User
            {
                Id = 1,
                Name = "test user",
                Roles = new List<Role>
                {
                    dbContext.Find<Role>(1),
                    dbContext.Find<Role>(2)
                },
                Addresses = new List<Address>
                {
                    new Address { Id = 1, Street = "original street", Number = "123" }
                },
                Profile = new UserProfile
                {
                    Id = 1,
                    FirstName = "test",
                    LastName = "user"
                },
                UserType = dbContext.Find<UserType>(1)
            };

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync();

            User savedUser = await dbContext.Users.Where(u => u.Id == 1)
                    .Include(u => u.Roles)
                    .Include(u => u.Addresses)
                    .Include(u => u.Profile)
                    .Include(u => u.UserType)
                    .FirstOrDefaultAsync();

            await dbContext.MapAsync<User>(new EditUserInput
            {
                Id = 1,
                Name = "edited name"
            });

            Assert.Equal(1, savedUser.Id);
            Assert.Equal("edited name", savedUser.Name);
            Assert.NotNull(savedUser.Profile);
            Assert.Equal("test", savedUser.Profile.FirstName);
            Assert.Equal("user", savedUser.Profile.LastName);
            Assert.NotNull(savedUser.Addresses);
            Assert.Single(savedUser.Addresses);
            Assert.Equal("original street", savedUser.Addresses[0].Street);
            Assert.Equal("123", savedUser.Addresses[0].Number);
            Assert.NotNull(savedUser.Roles);
            Assert.Equal(2, savedUser.Roles.Count);
            Assert.Contains(savedUser.Roles, r => r.Id == 1);
            Assert.Contains(savedUser.Roles, r => r.Id == 2);
            Assert.NotNull(savedUser.UserType);
            Assert.Equal(1, savedUser.UserType.Id);
        }

        public class EditUserInput
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual DateTime DateOfBirth { get; set; }

            public virtual List<Role> Roles { get; set; }

            [Composition]
            public virtual List<Address> Addresses { get; set; }

            public virtual UserType UserType { get; set; }

            [Composition]
            public virtual UserProfile Profile { get; set; }
        }

        public class UserProfile
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string FirstName { get; set; }

            public virtual string LastName { get; set; }
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

        public class UserType
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class Address
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }
        }

        public class InputTestDbContext : TestDbContext
        {
            public InputTestDbContext(DbContextOptions<InputTestDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }

            public DbSet<Role> Roles { get; set; }

            public DbSet<UserType> UserTypes { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>()
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