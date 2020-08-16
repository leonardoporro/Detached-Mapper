using Detached.EntityFramework.Tests.Context;
using Detached.EntityFramework.Tests.Model;
using Detached.EntityFramework.Tests.Model.DTOs;
using Detached.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Detached.EntityFramework.Tests
{
    public class MapEntityTests
    {
        [Fact]
        public async Task map_user()
        {
            Mapper mapper = new Mapper();
            TestDbContext db = await TestDbContext.CreateInMemorySqliteAsync(mapper);

            db.Roles.Add(new Role { Id = 1, Name = "admin" });
            db.Roles.Add(new Role { Id = 2, Name = "user" });
            db.UserTypes.Add(new UserType { Id = 1, Name = "system" });
            await db.SaveChangesAsync();

            await db.MapAsync<User>(new UserDTO
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

            await db.SaveChangesAsync();

            User user = await db.Users.Where(u => u.Id == 1)
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
    }
}