using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Metadata;
using EntityFrameworkCore.Detached.Tests.Model;
using EntityFrameworkCore.Detached.Tests.Model.ManyToMany;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests
{
    public class ManyToManyFeatureTests
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

                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                User persisted = await detachedContext.LoadAsync<User>(1);
                Assert.Equal(2, persisted.Roles.Count);
                Assert.True(persisted.Roles.Any(r => r.Name == "Admin"));
                Assert.True(persisted.Roles.Any(r => r.Name == "PowerUser"));
            }
        }

        [Fact]
        public async Task when_entity_is_saved__many_to_many_is_saved()
        {
            using (TestDbContext dbContext = new TestDbContext())
            {
                IDetachedContext detachedContext = new DetachedContext<TestDbContext>(dbContext);

                dbContext.AddRange(new[]
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                });
                await dbContext.SaveChangesAsync();

                await detachedContext.SaveAsync(new User
                {
                    Name = "U",
                    Roles = new[] { new Role { Id = 1 } }
                });

                User persisted = await detachedContext.LoadAsync<User>(1);
                Assert.Equal(1, persisted.Roles.Count);
                Assert.True(persisted.Roles.Any(r => r.Name == "Admin"));
            }
        }

        [Fact]
        public async Task when_entity_is_edited__many_to_many_is_merged()
        {
            using (TestDbContext dbContext = new TestDbContext())
            {
                dbContext.AddRange(new[]
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                });
                await dbContext.SaveChangesAsync();

                IDetachedContext<TestDbContext> detached = new DetachedContext<TestDbContext>(dbContext);
                await detached.SaveAsync(new User
                {
                    Name = "Test User",
                    Roles = new[]
                    {
                        new Role { Id = 1 },
                        new Role { Id = 2 }
                    }
                });

                await detached.SaveAsync(new User
                {
                    Id = 1,
                    Name = "Test User",
                    Roles = new[]
                    {
                        new Role { Id = 1 },
                    }
                });

                User persisted = await detached.LoadAsync<User>(1);
            }
        }
    }
}