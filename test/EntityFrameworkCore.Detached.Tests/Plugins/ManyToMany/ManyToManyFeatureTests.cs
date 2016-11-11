using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace EntityFrameworkCore.Detached.Tests.Plugins.ManyToMany
{
    public class ManyToManyFeatureTests
    {
        [Fact]
        public async Task when_entity_is_loaded__many_to_many_is_loaded()
        {
            using (ManyToManyContext dbContext = new ManyToManyContext())
            {
                dbContext.AddRange(new[]
                {
                    new Role { Name = "Role 1" },
                    new Role { Name = "Role 2" }
                });
                dbContext.SaveChanges();

                dbContext.Add(new User
                {
                    Name = "Test Root"
                });
                dbContext.SaveChanges();

                dbContext.AddRange(new[]
                {
                    new UserRoles { User = dbContext.User.First(), Role = dbContext.Roles.First() },
                    new UserRoles { User = dbContext.User.First(), Role = dbContext.Roles.Last()  }
                });
                dbContext.SaveChanges();

                IDetachedContext<ManyToManyContext> detachedContext = new DetachedContext<ManyToManyContext>(dbContext);

                User persisted = await detachedContext.LoadAsync<User>(1);
                Assert.Equal(2, persisted.UserRoles.Count);
                Assert.Equal(2, persisted.Roles.Count);
                Assert.True(persisted.Roles.Any(r => r.Name == "Role 1"));
                Assert.True(persisted.Roles.Any(r => r.Name == "Role 2"));
            }
        }

        [Fact]
        public async Task when_entity_is_saved__many_to_many_is_saved()
        {
            using (ManyToManyContext dbContext = new ManyToManyContext())
            {
                IDetachedContext<ManyToManyContext> detachedContext = new DetachedContext<ManyToManyContext>(dbContext);
                dbContext.AddRange(new[]
                {
                    new Role { Name = "Role 1" },
                    new Role { Name = "Role 2" }
                });
                await dbContext.SaveChangesAsync();

                await detachedContext.UpdateAsync(new User
                {
                    Name = "Test",
                    Roles = new[] {
                        new Role { Id = 1 }
                    }
                });
                await detachedContext.SaveChangesAsync();

                User persisted = await detachedContext.LoadAsync<User>(1);
                Assert.Equal(1, persisted.Roles.Count);
                Assert.Equal(1, persisted.UserRoles.Count);
                Assert.True(persisted.Roles.Any(r => r.Name == "Role 1"));
            }
        }

        [Fact]
        public async Task when_entity_is_edited__many_to_many_is_merged()
        {
            using (ManyToManyContext dbContext = new ManyToManyContext())
            {
                // GIVEN some entities of type B:
                dbContext.AddRange(new[]
                {
                    new Role { Name = "EndB 1" },
                    new Role { Name = "EndB 2" },
                    new Role { Name = "EndB 3" }
                });
                await dbContext.SaveChangesAsync();

                // AND a root A containing two associations of B.
                IDetachedContext<ManyToManyContext> detached = new DetachedContext<ManyToManyContext>(dbContext);
                await detached.UpdateAsync(new User
                {
                    Name = "Test Root",
                    Roles = new[]
                    {
                        new Role { Id = 1, Name = "Role 1" },
                        new Role { Id = 2, Name = "Role 2" }
                    }
                });
                await detached.SaveChangesAsync();

                // WHEN a root with only one association of B is saved:
                await detached.UpdateAsync(new User
                {
                    Id = 1,
                    Name = "Test Root",
                    Roles = new[]
                    {
                        new Role { Id = 3, Name = "Role 3" },
                    }
                });
                await detached.SaveChangesAsync();

                // THEN the association to the second item is removed:
                User persisted = await detached.LoadAsync<User>(1);
            }
        }
    }
}