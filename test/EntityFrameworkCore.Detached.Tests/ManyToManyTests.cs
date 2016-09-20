using EntityFrameworkCore.Detached.ManyToMany;
using EntityFrameworkCore.Detached.Tests.Model;
using EntityFrameworkCore.Detached.Tests.Model.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests
{
    public class ManyToManyTests
    {
        [Fact]
        public async Task when_entity_is_loaded__many_to_many_is_loaded()
        {
            using (TestDbContext context = new TestDbContext())
            {
                Role[] roles = new[]
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "PowerUser" },
                    new Role { Name = "User" }
                };
                context.AddRange(roles);

                User user = new User
                {
                    Name = "Test User"
                };
                context.Add(user);

                context.AddRange(new[]
                {
                    new ManyToManyEntity<User,Role> { End1 = user, End2 = roles[0] },
                    new ManyToManyEntity<User, Role> { End1 = user, End2 = roles[1] }
                });
                context.SaveChanges();

                DetachedContext detachedContext = new DetachedContext(context);

                User persisted = await detachedContext.LoadAsync<User>(1);
                Assert.Equal(2, persisted.Roles.Count);
            }
        }
    }
}
