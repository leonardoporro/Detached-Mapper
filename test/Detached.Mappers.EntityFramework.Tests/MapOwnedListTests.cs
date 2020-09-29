using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Mappers.EntityFramework.Tests
{
    public class MapOwnedListTests
    {
        [Fact]
        public async Task map_owned_list()
        {
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync();

            db.Roles.Add(new Role { Name = "admin" });
            db.Roles.Add(new Role { Name = "user" });
            db.UserTypes.Add(new UserType { Name = "system" });
            await db.SaveChangesAsync();

            User user = new User
            {
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

            db.Users.Add(user);
            await db.SaveChangesAsync();

            User savedUser = await db.Users.Where(u => u.Id == 1)
                  .Include(u => u.Roles)
                  .Include(u => u.Addresses)
                  .Include(u => u.Profile)
                  .Include(u => u.UserType)
                  .FirstOrDefaultAsync();

            await db.MapAsync<User>(new EditUserInput
            {
                Id = 1,
                Addresses = new List<Address>
                {
                    new Address { Street = "new street", Number = "234" }
                }
            });

            Assert.Equal(1, user.Id);
            Assert.Equal("test user", user.Name);
            Assert.NotNull(user.Profile);
            Assert.Equal("test", user.Profile.FirstName);
            Assert.Equal("user", user.Profile.LastName);
            Assert.NotNull(user.Addresses);
            Assert.Equal(1, user.Addresses.Count);
            Assert.Equal("new street", user.Addresses[0].Street);
            Assert.Equal("234", user.Addresses[0].Number);
            Assert.NotNull(user.Roles);
            Assert.Equal(2, user.Roles.Count);
            Assert.Contains(user.Roles, r => r.Id == 1);
            Assert.Contains(user.Roles, r => r.Id == 2);
            Assert.NotNull(user.UserType);
            Assert.Equal(1, user.UserType.Id);
        }

        public class EditUserInput
        {
            public int Id { get; set; }

            public List<Address> Addresses { get; set; }
        }
    }
}