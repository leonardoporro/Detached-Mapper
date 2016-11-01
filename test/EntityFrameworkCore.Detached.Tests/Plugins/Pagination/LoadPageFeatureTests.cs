using EntityFrameworkCore.Detached.Plugins.Pagination;
using EntityFrameworkCore.Detached.Tests.Model;
using EntityFrameworkCore.Detached.Tools;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests.Plugins.Pagination
{
    public class LoadPageFeatureTests
    {
        [Fact]
        public async Task when_a_page_is_requested__the_page_is_loaded()
        {
            using (TestDbContext dbContext = new TestDbContext())
            {
                // GIVEN a list of entities:
                for (int i = 1; i <= 50; i++)
                {
                    dbContext.Entities.Add(new Entity
                    {
                        Name = "Entity " + i.ToString("D2")
                    });
                }
                await dbContext.SaveChangesAsync();

                // WHEN a page of size 10 is loaded:
                DetachedContext<TestDbContext> detached = new DetachedContext<Model.TestDbContext>(dbContext);

                IPaginatedResult<Entity> result = await detached.LoadPageAsync(new PaginatedRequest<Entity>
                {
                    PageIndex = 1,
                    PageSize = 10
                });

                // THEN page contains 10 items and correct indexes and count values:
                Assert.Equal(10, result.Items.Count);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(50, result.RowCount);
                Assert.Equal(5, result.PageCount);
            }
        }

        [Fact]
        public async Task when_an_ordered_page_is_requested__the_page_is_loaded()
        {
            using (TestDbContext dbContext = new TestDbContext())
            {
                // GIVEN a list of entities:
                for (int i = 1; i <= 50; i++)
                {
                    dbContext.Entities.Add(new Entity
                    {
                        Name = "Entity " + i.ToString("D2")
                    });
                }
                await dbContext.SaveChangesAsync();

                // WHEN a page of size 10 is loaded:
                DetachedContext<TestDbContext> detached = new DetachedContext<Model.TestDbContext>(dbContext);

                IPaginatedResult<Entity> result = await detached.LoadPageAsync(new PaginatedRequest<Entity>
                {
                    PageIndex = 1,
                    PageSize = 10,
                    Sorter = new Sorter<Entity>().OrderByDescending(e => e.Name)
                });

                // THEN page contains 10 items and correct indexes and count values:
                Assert.Equal(10, result.Items.Count);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(50, result.RowCount);
                Assert.Equal(5, result.PageCount);
                Assert.Equal("Entity 50", result.Items.First().Name);
            }
        }

        [Fact]
        public async Task when_an_filtered_page_is_requested__the_page_is_loaded()
        {
            using (TestDbContext dbContext = new TestDbContext())
            {
                // GIVEN a list of entities:
                for (int i = 1; i <= 50; i++)
                {
                    dbContext.Entities.Add(new Entity
                    {
                        Name = "Entity " + i.ToString("D2")
                    });
                }
                await dbContext.SaveChangesAsync();

                // WHEN a page of size 10 is loaded:
                DetachedContext<TestDbContext> detached = new DetachedContext<Model.TestDbContext>(dbContext);

                IPaginatedResult<Entity> result = await detached.LoadPageAsync<Entity>(new PaginatedRequest<Entity>
                {
                    PageIndex = 1,
                    PageSize = 10,
                    Filter = e => string.Compare(e.Name, "Entity 20") > 0
                });

                // THEN page contains 10 items and correct indexes and count values:
                Assert.Equal(10, result.Items.Count);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(30, result.RowCount);
                Assert.Equal(3, result.PageCount);
                Assert.Equal("Entity 21", result.Items.First().Name);
            }
        }

        [Fact]
        public async Task when_a_transformed_page_is_requested__the_page_is_loaded()
        {
            using (TestDbContext dbContext = new TestDbContext())
            {
                // GIVEN a list of entities:
                for (int i = 1; i <= 50; i++)
                {
                    dbContext.Entities.Add(new Entity
                    {
                        Name = "Entity " + i.ToString("D2")
                    });
                }
                await dbContext.SaveChangesAsync();

                // WHEN a page of size 10 is loaded:
                DetachedContext<TestDbContext> detached = new DetachedContext<Model.TestDbContext>(dbContext);

                IPaginatedResult<Item> result = await detached.LoadPageAsync(new PaginatedRequest<Entity>
                {
                    PageIndex = 1,
                    PageSize = 10,
                    Filter = e => string.Compare(e.Name, "Entity 20") > 0
                }, e => new Item { Id = e.Id, Description = e.Name });

                // THEN page contains 10 transformed items and correct indexes and count values:
                Assert.Equal(10, result.Items.Count);
                Assert.Equal(10, result.PageSize);
                Assert.Equal(1, result.PageIndex);
                Assert.Equal(30, result.RowCount);
                Assert.Equal(3, result.PageCount);
                Assert.Equal("Entity 21", result.Items.First().Description);
            }
        }
    }
}
