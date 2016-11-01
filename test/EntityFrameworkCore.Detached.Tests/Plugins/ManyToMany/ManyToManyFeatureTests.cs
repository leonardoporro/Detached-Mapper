using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests.Plugins.ManyToMany
{
    public class ManyToManyFeatureTests
    {
        [Fact]
        public async Task Saraasa()
        {
            using (ManyToManyContext context = new ManyToManyContext())
            {
                context.User.Add(new User
                {

                });
                await context.SaveChangesAsync();
            }

            //        [Fact]
            //        public async Task when_entity_is_loaded__many_to_many_is_loaded()
            //        {
            //            using (TestDbContext dbContext = new TestDbContext())
            //            {
            //                ManyToManyEndB[] roles = new[]
            //                {
            //                    new ManyToManyEndB { Name = "EndB 1" },
            //                    new ManyToManyEndB { Name = "EndB 2" },
            //                    new ManyToManyEndB { Name = "EndB 3" }
            //                };
            //                dbContext.AddRange(roles);

            //                ManyToManyEndA user = new ManyToManyEndA
            //                {
            //                    Name = "Test Root"
            //                };
            //                dbContext.Add(user);

            //                dbContext.AddRange(new[]
            //                {
            //                    new ManyToManyEntity<ManyToManyEndA,ManyToManyEndB> { End1 = user, End2 = roles[0] },
            //                    new ManyToManyEntity<ManyToManyEndA, ManyToManyEndB> { End1 = user, End2 = roles[1] }
            //                });
            //                dbContext.SaveChanges();

            //                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(dbContext);

            //                ManyToManyEndA persisted = await detachedContext.LoadAsync<ManyToManyEndA>(1);
            //                Assert.Equal(2, persisted.EndB.Count);
            //                Assert.True(persisted.EndB.Any(r => r.Name == "EndB 1"));
            //                Assert.True(persisted.EndB.Any(r => r.Name == "EndB 2"));
            //            }
            //        }

            //        [Fact]
            //        public async Task when_entity_is_saved__many_to_many_is_saved()
            //        {
            //            using (TestDbContext dbContext = new TestDbContext())
            //            {
            //                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(dbContext);

            //                dbContext.AddRange(new[]
            //                {
            //                    new ManyToManyEndB { Name = "EndB 1" },
            //                    new ManyToManyEndB { Name = "EndB 2" }
            //                });
            //                await dbContext.SaveChangesAsync();

            //                await detachedContext.UpdateAsync(new ManyToManyEndA
            //                {
            //                    Name = "Test",
            //                    EndB = new[] { new ManyToManyEndB { Id = 1 } }
            //                });
            //                await detachedContext.SaveChangesAsync();

            //                ManyToManyEndA persisted = await detachedContext.LoadAsync<ManyToManyEndA>(1);
            //                Assert.Equal(1, persisted.EndB.Count);
            //                Assert.True(persisted.EndB.Any(r => r.Name == "EndB 1"));
            //            }
            //        }

            //        [Fact]
            //        public async Task when_entity_is_edited__many_to_many_is_merged()
            //        {
            //            using (TestDbContext dbContext = new TestDbContext())
            //            {
            //                // GIVEN some entities of type B:
            //                dbContext.AddRange(new[]
            //                {
            //                    new ManyToManyEndB { Name = "EndB 1" },
            //                    new ManyToManyEndB { Name = "EndB 2" },
            //                    new ManyToManyEndB { Name = "EndB 3" }
            //                });
            //                await dbContext.SaveChangesAsync();

            //                // AND a root A containing two associations of B.
            //                IDetachedContext<TestDbContext> detached = new DetachedContext<TestDbContext>(dbContext);
            //                await detached.UpdateAsync(new ManyToManyEndA
            //                {
            //                    Name = "Test Root",
            //                    EndB = new[]
            //                    {
            //                        new ManyToManyEndB { Id = 1, Name = "EndB 1" },
            //                        new ManyToManyEndB { Id = 2, Name = "EndB 2" }
            //                    }
            //                });
            //                await detached.SaveChangesAsync();

            //                // WHEN a root with only one association of B is saved:
            //                await detached.UpdateAsync(new ManyToManyEndA
            //                {
            //                    Id = 1,
            //                    Name = "Test Root",
            //                    EndB = new[]
            //                    {
            //                        new ManyToManyEndB { Id = 3, Name = "EndB 3" },
            //                    }
            //                });
            //                await detached.SaveChangesAsync();

            //                // THEN the association to the second item is removed:
            //                ManyToManyEndA persisted = await detached.LoadAsync<ManyToManyEndA>(1);
            //            }
            //        }
        }
    }
}