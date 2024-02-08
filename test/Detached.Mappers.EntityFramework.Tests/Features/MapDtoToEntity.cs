using Detached.Annotations;
using Detached.Mappers;
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
    public class MapDtoToEntity
    {
        [Fact]
        public async Task map_dto_to_entity()
        {
            var dbContext = await TestDbContext.Create<EntityTestDbContext>();

            dbContext.Roles.Add(new Role { Id = 1, Name = "admin" });
            dbContext.Roles.Add(new Role { Id = 2, Name = "user" });
            dbContext.UserTypes.Add(new UserType { Id = 1, Name = "system" });
            dbContext.SaveChanges();

            await dbContext.MapAsync<User>(new UserDto
            {
                Id = 1,
                Name = "cr",
                Profile = new UserProfileDto
                {
                    FirstName = "chris",
                    LastName = "redfield"
                },
                Addresses = new List<AddressDto>
                {
                    new AddressDto { Street = "rc", Number = "123" }
                },
                Roles = new List<RoleDto>
                {
                    new RoleDto { Id = 1 },
                    new RoleDto { Id = 2 }
                },
                UserType = new UserTypeDto { Id = 1 }
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
        public async Task map_dto_to_entity_notfound()
        {
            var dbContext = await TestDbContext.Create<EntityTestDbContext>();

            await Assert.ThrowsAsync<MapperException>(() =>
                dbContext.MapAsync<User>(new UserDto
                {
                    Id = 1,
                    Name = "cr",
                },
                new MapParameters
                {
                    MissingRootBehavior = MissingRootBehavior.ThrowException
                })
            );
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

        public class UserDto
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<RoleDto> Roles { get; set; }

            public virtual List<AddressDto> Addresses { get; set; }

            public virtual UserTypeDto UserType { get; set; }

            public virtual UserProfileDto Profile { get; set; }
        }

        public class UserProfile
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string FirstName { get; set; }

            public virtual string LastName { get; set; }
        }

        public class UserProfileDto
        {
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

        public class RoleDto
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
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual List<User> Users { get; set; }
        }

        public class UserTypeDto
        {
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }
        }

        public class Address
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }
        }

        public class AddressDto
        {
            public virtual int Id { get; set; }

            public virtual string Street { get; set; }

            public virtual string Number { get; set; }
        }

        public class EntityTestDbContext : TestDbContext
        {
            public EntityTestDbContext(DbContextOptions<EntityTestDbContext> options)
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