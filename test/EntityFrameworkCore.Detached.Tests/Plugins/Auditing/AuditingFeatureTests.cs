using EntityFrameworkCore.Detached.Tests.Model;
using EntityFrameworkCore.Detached.Tests.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests.Plugins.Auditing
{
    public class AuditingPluginFeatureTests
    {
        [Fact]
        public async Task when_an_entity_with_audit_is_created__the_created_audit_fields_are_persisted()
        {
            TestDbContext dbContext = new TestDbContext();
            IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(new TestDbContext());

            SessionInfoProvider.Default.CurrentUser = "Test User";
            await detachedContext.UpdateAsync(new EntityForAuditing
            {
                Name = "Test"
            });
            await detachedContext.SaveChangesAsync();

            EntityForAuditing persisted = await detachedContext.LoadAsync<EntityForAuditing>(1);
            Assert.Equal("Test User", persisted.CreatedBy);
            Assert.Equal(DateTime.Now.Date, persisted.CreatedDate.Date);
        }

        [Fact]
        public async Task when_an_entity_with_audit_is_modified__the_modified_audit_fields_are_persisted()
        {
            // GIVEN a persisted entity
            TestDbContext dbContext = new TestDbContext();
            IDetachedContext<TestDbContext> context = new DetachedContext<TestDbContext>(dbContext);

            await context.UpdateAsync(new EntityForAuditing
            {
                Name = "Test"
            });
            await context.SaveChangesAsync();

            EntityForAuditing persisted = await context.LoadAsync<EntityForAuditing>(1);

            // WHEN the entity is modified
            SessionInfoProvider.Default.CurrentUser = "Test User 2";
            await context.UpdateAsync(new EntityForAuditing
            {
                Id = 1,
                Name = "Test Modified"
            });
            await context.SaveChangesAsync();

            EntityForAuditing persisted2 = await context.LoadAsync<EntityForAuditing>(1);

            // THEN 'created' audit fields are not modified.
            Assert.Equal(persisted.CreatedDate, persisted2.CreatedDate);
            Assert.Equal(persisted.CreatedBy, persisted2.CreatedBy);
            // and 'modified' audit fields are set.
            Assert.Equal("Test User 2", persisted2.ModifiedBy);
            Assert.Equal(DateTime.Now.Date, persisted2.ModifiedDate.Date);
        }
    }
}
