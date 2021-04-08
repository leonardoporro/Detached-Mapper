using Detached.Mappers.EntityFramework.Tests.Context;
using Detached.Mappers.EntityFramework.Tests.Model;
using System.Collections.Generic;
using System.Data.Entity;
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
            TestDbContext db = await TestDbContext.CreateAsync();
            db.Roles.Add(new Role { Id = 1, Name = "admin" });
            db.Roles.Add(new Role { Id = 2, Name = "user" });
            await db.SaveChangesAsync();

            User user = await db.MapAsync<User>(new NoKeyUserDTO
            {
                Name = "nokeyuser",
                Roles = new List<Role>
                {
                    new Role { Id = 1 },
                    new Role { Id = 2 }
                }
            });

            await db.SaveChangesAsync();

            User persisted = db.Users.Include(u => u.Roles)
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
    }
}