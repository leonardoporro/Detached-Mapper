using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Tests.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests
{
    public class DetachedContextFeatureTests
    {
        [Fact]
        public async Task when_item_is_added_to_owned_collection__item_is_created()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with an owned list:
                context.Add(new Entity
                {
                    OwnedList = new[]  {
                        new OwnedListItem { Id = 1, Name = "Owned Item A" }
                    }
                });
                context.SaveChanges();

                // WHEN the collection is modified:
                Entity detachedEntity = new Entity
                {
                    Id = 1,
                    OwnedList = new[]  {
                        new OwnedListItem { Id = 1, Name = "Owned Item A" },
                        new OwnedListItem { Id = 2, Name = "Owned Item B" }
                    }
                };

                await detachedContext.UpdateAsync(detachedEntity);
                await detachedContext.SaveChangesAsync();

                // THEN the items is added to the the database.
                Entity persistedEntity = await detachedContext.LoadAsync<Entity>(1);
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item A"));
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item B"));
            }
        }

        [Fact]
        public async Task when_item_is_removed_from_owned_collection__item_is_deleted()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with an owned list:
                context.Add(new Entity
                {
                    OwnedList = new[]  {
                            new OwnedListItem { Id = 1, Name = "Owned Item A" },
                            new OwnedListItem { Id = 2, Name = "Owned Item B" }
                        }
                });
                context.SaveChanges();

                // WHEN the collection is modified:
                Entity detachedEntity = new Entity
                {
                    Id = 1,
                    OwnedList = new[]  {
                            new OwnedListItem { Id = 2, Name = "Owned Item B" },
                        }
                };

                await detachedContext.UpdateAsync(detachedEntity);
                await detachedContext.SaveChangesAsync();

                // THEN the items is added to the the database.
                Entity persistedEntity = await detachedContext.LoadAsync<Entity>(1);
                Assert.False(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item A"));
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item B"));
            }
        }

        [Fact]
        public async Task when_owned_property_is_set_to_entity__entity_is_created()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with references:
                context.Add(new Entity
                {
                    OwnedReference = new OwnedReference { Id = 1, Name = "Owned Reference 1" }
                });
                context.SaveChanges();

                // WHEN the owned and the associated references are set to null:
                Entity detachedEntity = new Entity
                {
                    Id = 1,
                    OwnedReference = new OwnedReference { Id = 1, Name = "Owned Reference 2" }
                };

                await detachedContext.UpdateAsync(detachedEntity);
                await detachedContext.SaveChangesAsync();

                // THEN the owned reference is replaced:
                Assert.False(context.OwnedReferences.Any(o => o.Name == "Owned Reference 1"));
            }
        }

        [Fact]
        public async Task when_owned_property_is_set_to_null__entity_is_deleted()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with references:
                context.Add(new Entity
                {
                    OwnedReference = new OwnedReference { Id = 1, Name = "Owned Reference 1" }
                });
                context.SaveChanges();

                // WHEN the owned and the associated references are set to null:
                Entity detachedEntity = new Entity
                {
                    Id = 1,
                    OwnedReference = null
                };

                await detachedContext.UpdateAsync(detachedEntity);
                await detachedContext.SaveChangesAsync();

                // THEN the owned reference is removed:
                Assert.False(context.OwnedReferences.Any(o => o.Name == "Owned Reference 1"));
            }
        }

        [Fact]
        public async Task when_associated_property_is_set_to_entity__entity_is_related_to_existing()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with references:
                AssociatedReference[] references = new[]
                {
                        new AssociatedReference { Id = 1, Name = "Associated Reference 1" },
                        new AssociatedReference { Id = 2, Name = "Associated Reference 2" }
                    };
                context.AddRange(references);
                context.Add(new Entity
                {
                    AssociatedReference = references[0],
                });
                await context.SaveChangesAsync();

                // WHEN the owned and the associated references are set to null:
                Entity detachedEntity = new Entity
                {
                    Id = 1,
                    AssociatedReference = new AssociatedReference { Id = 1, Name = "Modified Associated Reference 1" },
                };

                await detachedContext.UpdateAsync(detachedEntity);
                await detachedContext.SaveChangesAsync();

                // THEN the associated reference still exsits:
                Assert.True(context.AssociatedReferences.Any(a => a.Name == "Associated Reference 1"));
            }
        }

        [Fact]
        public async Task when_associated_property_is_set_to_null__entity_is_preserved()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with references:
                AssociatedReference[] references = new[]
                {
                        new AssociatedReference { Id = 1, Name = "Associated Reference 1" },
                        new AssociatedReference { Id = 2, Name = "Associated Reference 2" }
                    };
                context.AddRange(references);
                context.Add(new Entity
                {
                    AssociatedReference = references[0],
                });
                context.SaveChanges();

                // WHEN the owned and the associated references are set to null:
                Entity detachedEntity = new Entity
                {
                    Id = 1,
                    AssociatedReference = null,
                    OwnedReference = null
                };

                await detachedContext.UpdateAsync(detachedEntity);
                await detachedContext.SaveChangesAsync();

                // THEN the associated reference still exsits:
                Assert.True(context.AssociatedReferences.Any(a => a.Name == "Associated Reference 1"));
            }
        }

        [Fact]
        public async Task when_entity_is_deleted__owned_properties_are_deleted()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                // GIVEN an enity root with an owned list:
                var associatedItems = new List<AssociatedListItem>(new[]
                {
                    new AssociatedListItem { Name = "Associated Item 1" },
                    new AssociatedListItem { Name = "Associated Item 2" }
                });
                context.AddRange(associatedItems);
                await context.SaveChangesAsync();

                context.Add(new Entity
                {
                    OwnedList = new List<OwnedListItem>(new[]  {
                        new OwnedListItem { Name = "Owned Item A" },
                        new OwnedListItem { Name = "Owned Item B" },
                        new OwnedListItem { Name = "Owned Item C" }
                    }),
                    AssociatedList = associatedItems
                });
                context.SaveChanges();

                // WHEN the entity is deleted:
                await detachedContext.DeleteAsync(new Entity { Id = 1 });
                await detachedContext.SaveChangesAsync();

                // THEN owned items are removed:
                Assert.False(context.OwnedListItems.Any(e => e.Name == "Owned Item A"));
                Assert.False(context.OwnedListItems.Any(e => e.Name == "Owned Item B"));
                Assert.False(context.OwnedListItems.Any(e => e.Name == "Owned Item C"));

                // and the associated items are not removed:
                Assert.True(context.AssociatedListItems.Any(e => e.Name == "Associated Item 1"));
                Assert.True(context.AssociatedListItems.Any(e => e.Name == "Associated Item 2"));
            }
        }

        [Fact]
        public async Task when_entity_is_loaded_by_key__type_conversion_is_made()
        {
            using (TestDbContext context = new TestDbContext())
            {
                IDetachedContext<TestDbContext> detachedContext = new DetachedContext<TestDbContext>(context);

                await detachedContext.UpdateAsync(new Entity
                {
                    Name = "Test entity"
                });
                await detachedContext.SaveChangesAsync();


                Entity persisted = await detachedContext.LoadAsync<Entity>("1");
                Assert.NotNull(persisted);

                persisted = await detachedContext.LoadAsync<Entity>(1);
                Assert.NotNull(persisted);
            }
        }
    }
}