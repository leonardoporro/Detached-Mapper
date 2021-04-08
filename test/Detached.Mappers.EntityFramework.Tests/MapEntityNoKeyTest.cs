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

            User user = await db.MapAsync<User>(new NoKeyUserDTO
            {
                Name = "nokeyuser",
                Roles = new List<NoKeyUserRole>
                {
                    new NoKeyUserRole{ Name = "Admin" }
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

            public virtual List<NoKeyUserRole> Roles { get; set; }
        }

        public class NoKeyUserRole
        {
            public string Name { get; set; }
        }
    }
}