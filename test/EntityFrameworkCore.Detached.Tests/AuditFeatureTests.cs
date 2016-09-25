using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Tests.Model;
using EntityFrameworkCore.Detached.Tests.Model.Audit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests
{
    public class AuditFeatureTests
    {
        [Fact]
        public async Task when_an_entity_with_audit_is_created__the_created_audit_fields_are_persisted()
        {
            TestDbContext dbContext = new TestDbContext();
            IDetachedSessionInfoProvider sessionProvider = new DelegateSessionInfoProvider(() => "Test User");
            IDetachedContext<TestDbContext> context = new DetachedContext<TestDbContext>(new TestDbContext(), sessionProvider);

            await context.UpdateAsync(new EntityForAudit
            {
                Name = "Test"
            });
            await context.SaveChangesAsync();

            EntityForAudit persisted = await context.LoadAsync<EntityForAudit>(1);
            Assert.Equal("Test User", persisted.CreatedBy);
            Assert.Equal(DateTime.Now.Date, persisted.CreatedDate.Date);
        }

        [Fact]
        public async Task when_an_entity_with_audit_is_modified__the_modified_audit_fields_are_persisted()
        {
            // GIVEN a persisted entity
            TestDbContext dbContext = new TestDbContext();
            string userName = "Test User";
            IDetachedSessionInfoProvider sessionProvider = new DelegateSessionInfoProvider(() => userName);
            IDetachedContext<TestDbContext> context = new DetachedContext<TestDbContext>(dbContext, sessionProvider);

            await context.UpdateAsync(new EntityForAudit
            {
                Name = "Test"
            });
            await context.SaveChangesAsync();

            EntityForAudit persisted = await context.LoadAsync<EntityForAudit>(1);

            // WHEN the entity is modified
            userName = "Test User 2";
            await context.UpdateAsync(new EntityForAudit
            {
                Id = 1,
                Name = "Test Modified"
            });
            await context.SaveChangesAsync();

            EntityForAudit persisted2 = await context.LoadAsync<EntityForAudit>(1);

            // THEN 'created' audit fields are not modified.
            Assert.Equal(persisted.CreatedDate, persisted2.CreatedDate);
            Assert.Equal(persisted.CreatedBy, persisted2.CreatedBy);
            // and 'modified' audit fields are set.
            Assert.Equal("Test User 2", persisted2.ModifiedBy);
            Assert.Equal(DateTime.Now.Date, persisted2.ModifiedDate.Date);
        }
    }
}
