using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.EntityFramework.Tests.Plugins.Auditing
{
    public class AuditingPluginFeatureTests
    {
        [Fact]
        public async Task when_an_entity_with_audit_is_created__the_created_audit_fields_are_persisted()
        {
            using (IDetachedContext<AuditingContext> detachedContext = new DetachedContext<AuditingContext>())
            {
                SessionInfoProvider.Default.CurrentUser = "Test User";
                await detachedContext.Set<EntityForAuditing>().UpdateAsync(new EntityForAuditing
                {
                    Name = "Test"
                });
                await detachedContext.SaveChangesAsync();

                EntityForAuditing persisted = await detachedContext.Set<EntityForAuditing>().LoadAsync(1);
                Assert.Equal("Test User", persisted.CreatedBy);
                Assert.Equal(DateTime.Now.Date, persisted.CreatedDate.Date);
            }
        }

        [Fact]
        public async Task when_an_entity_with_audit_is_modified__the_modified_audit_fields_are_persisted()
        {
            // GIVEN a persisted entity:
            using (IDetachedContext<AuditingContext> detachedContext = new DetachedContext<AuditingContext>())
            {

                await detachedContext.Set<EntityForAuditing>().UpdateAsync(new EntityForAuditing
                {
                    Name = "Test"
                });
                await detachedContext.SaveChangesAsync();

                EntityForAuditing persisted = await detachedContext.Set<EntityForAuditing>().LoadAsync(1);

                // WHEN the entity is modified
                SessionInfoProvider.Default.CurrentUser = "Test User 2";
                await detachedContext.Set<EntityForAuditing>().UpdateAsync(new EntityForAuditing
                {
                    Id = 1,
                    Name = "Test Modified"
                });
                await detachedContext.SaveChangesAsync();

                EntityForAuditing persisted2 = await detachedContext.Set<EntityForAuditing>().LoadAsync(1);

                // THEN 'created' audit fields are not modified.
                Assert.Equal(persisted.CreatedDate, persisted2.CreatedDate);
                Assert.Equal(persisted.CreatedBy, persisted2.CreatedBy);
                // and 'modified' audit fields are set.
                Assert.Equal("Test User 2", persisted2.ModifiedBy);
                Assert.Equal(DateTime.Now.Date, persisted2.ModifiedDate.Date);
            }
        }
    }
}
