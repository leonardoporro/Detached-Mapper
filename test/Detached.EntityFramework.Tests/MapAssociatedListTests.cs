using Detached.EntityFramework.Tests.Context;
using Detached.EntityFramework.Tests.Model;
using Detached.Mapping;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.EntityFramework.Tests
{
    public class MapAssociatedListTests
    {
        [Fact]
        public async Task map_associated_list()
        {
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync();

            db.Roles.Add(new Role { Id = 1, Name = "admin" });
            db.Roles.Add(new Role { Id = 2,  Name = "user" });
            db.UserTypes.Add(new UserType { Id = 1, Name = "system" });
            await db.SaveChangesAsync();

            User newUser = new User
            {
                Id = 1,
                Name = "test user",
                Roles = new List<Role>
                {
                    db.Find<Role>(1),
                    db.Find<Role>(2)
                },
                Addresses = new List<Address>
                {
                    new Address { Street = "original street", Number = "123" }
                },
                Profile = new UserProfile
                {
                    FirstName = "test",
                    LastName = "user"
                },
                UserType = db.Find<UserType>(1)
            };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
             
            User mappedUser = await db.MapAsync<User>(new EditUserInput
            {
                Id = 1,
                Roles = new List<Role>
                {
                    new Role { Id = 1 }
                }
            });
            await db.SaveChangesAsync();

            User savedUser = db.Users.Where(u => u.Id == 1).Include(u => u.Roles).FirstOrDefault();

            Assert.Equal(1, savedUser.Id);
            Assert.Equal("test user", savedUser.Name);
            Assert.NotNull(savedUser.Profile);
            Assert.Equal("test", savedUser.Profile.FirstName);
            Assert.Equal("user", savedUser.Profile.LastName);
            Assert.NotNull(savedUser.Addresses);
            Assert.Equal(1, savedUser.Addresses.Count);
            Assert.Equal("original street", savedUser.Addresses[0].Street);
            Assert.Equal("123", savedUser.Addresses[0].Number);
            Assert.NotNull(savedUser.Roles);
            Assert.Equal(1, savedUser.Roles.Count);
            Assert.Contains(savedUser.Roles, r => r.Id == 1);
            Assert.NotNull(savedUser.UserType);
            Assert.Equal(1, savedUser.UserType.Id);
        }

        public class EditUserInput
        {
            public int Id { get; set; }

            public List<Role> Roles { get; set; }
        }
    }
}