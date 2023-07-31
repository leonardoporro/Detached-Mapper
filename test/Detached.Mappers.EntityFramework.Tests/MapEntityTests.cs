using Detached.Annotations;
using Detached.Mappers.EntityFramework.Tests.Fixture;
using Detached.Mappers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    [Collection("Default")]
    public class MapEntityTests
    {
        [Fact]
        public async Task map_entity()
        {
            EntityTestDbContext dbContext = new(); 

            dbContext.Roles.Add(new Role { Id = 1, Name = "admin" });
            dbContext.Roles.Add(new Role { Id = 2, Name = "user" });
            dbContext.UserTypes.Add(new UserType { Id = 1, Name = "system" });
            dbContext.SaveChanges();

            await dbContext.MapAsync<User>(new UserDTO
            {
                Id = 1,
                Name = "cr",
                Profile = new UserProfileDTO
                {
                    FirstName = "chris",
                    LastName = "redfield"
                },
                Addresses = new List<AddressDTO>
                {
                    new AddressDTO { Street = "rc", Number = "123" }
                },
                Roles = new List<RoleDTO>
                {
                    new RoleDTO { Id = 1 },
                    new RoleDTO { Id = 2 }
                },
                UserType = new UserTypeDTO { Id = 1 }
            });

            await dbContext.SaveChangesAsync();

            User user = await dbContext.Users.Where(u => u.Id == 1)
                    .Include(u => u.Roles)
                    .Include(u => u.Addresses)
                    .Include(u => u.Profile)
                    .Include(u => u.UserType)
                    .FirstOrDefaultAsync();

            Assert.Equal(1, user.Id);
            Assert.Equal("cr", user.Name);
            Assert.NotNull(user.Profile);
            Assert.Equal("chris", user.Profile.FirstName);
            Assert.Equal("redfield", user.Profile.LastName);
            Assert.NotNull(user.Addresses);
            Assert.Equal("rc", user.Addresses[0].Street);
            Assert.Equal("123", user.Addresses[0].Number);
            Assert.NotNull(user.Roles);
            Assert.Equal(2, user.Roles.Count);
            Assert.Contains(user.Roles, r => r.Id == 1);
            Assert.Contains(user.Roles, r => r.Id == 2);
            Assert.NotNull(user.UserType);
            Assert.Equal(1, user.UserType.Id);
        }

        [Fact]
        public async Task map_entity_not_found()
        {
            EntityTestDbContext dbContext = new();

            await Assert.ThrowsAsync<MapperException>(() =>
                dbContext.MapAsync<User>(new UserDTO
                {
                    Id = 1,
                    Name = "cr",
                },
                new MapParameters
                {
                    Upsert = false
                })
            );
        }

        public class User
        {
            [Key]
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

        public class UserDTO
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<RoleDTO> Roles { get; set; }

            public virtual List<AddressDTO> Addresses { get; set; }

            public virtual UserTypeDTO UserType { get; set; }

            public virtual UserProfileDTO Profile { get; set; }
        }

        public class UserProfile
        {
            [Key]
            public virtual int Id { get; set; }

            public virtual string FirstName { get; set; }

            public virtual string LastName { get; set; }
        }

        public class UserProfileDTO
        {
            public virtual int Id { get; set; }

            public virtual string FirstName { get; set; }

            public virtual string LastName { get; set; }
        }

        public class Role
        {
            [Key]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class RoleDTO
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }
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
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class UserTypeDTO
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }
        }

        public class Address
        {
            [Key]
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }
        }

        public class AddressDTO
        {
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }
        }

        public class EntityTestDbContext : TestDbContext
        {
            public DbSet<User> Users { get; set; }

            public DbSet<Role> Roles { get; set; }

            public DbSet<UserType> UserTypes { get; set; }
             
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