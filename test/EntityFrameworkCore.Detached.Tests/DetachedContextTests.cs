using EntityFrameworkCore.Detached.Tests.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests
{
    public class DetachedContextTests
    {
        [Fact(DisplayName = "Owned Collection: Add Item")]
        public async Task OwnedCollectionAddItem()
        {
            using (TestDbContext context = new TestDbContext())
            {
                DetachedContext detachedContext = new DetachedContext(context);

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

                await detachedContext.SaveAsync(detachedEntity);

                // THEN the items is added to the the database.
                Entity persistedEntity = detachedContext.Roots<Entity>().Single(r => r.Id == 1);
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item A"));
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item B"));
            }
        }

        [Fact(DisplayName = "Owned Collection: Remove Item")]
        public async Task OwnedCollectionRemoveItem()
        {
            using (TestDbContext context = new TestDbContext())
            {
                DetachedContext detachedContext = new DetachedContext(context);

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

                await detachedContext.SaveAsync(detachedEntity);

                // THEN the items is added to the the database.
                Entity persistedEntity = detachedContext.Roots<Entity>().Single(r => r.Id == 1);
                Assert.False(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item A"));
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item B"));
            }
        }

        [Fact(DisplayName = "Owned Reference: Set")]
        public async Task OwnedRefereceSet()
        {
            using (TestDbContext context = new TestDbContext())
            {
                DetachedContext detachedContext = new DetachedContext(context);

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

                await detachedContext.SaveAsync(detachedEntity);

                // THEN the owned reference is replaced:
                Assert.False(context.OwnedReferences.Any(o => o.Name == "Owned Reference 1"));
            }
        }

        [Fact(DisplayName = "Owned Reference: Remove")]
        public async Task OwnedReferenceRemove()
        {
            using (TestDbContext context = new TestDbContext())
            {
                DetachedContext detachedContext = new DetachedContext(context);

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

                await detachedContext.SaveAsync(detachedEntity);

                // THEN the owned reference is removed:
                Assert.False(context.OwnedReferences.Any(o => o.Name == "Owned Reference 1"));
            }
        }

        [Fact(DisplayName = "Associated Reference: Set")]
        public async Task AssociatedReferenceSet()
        {
            using (TestDbContext context = new TestDbContext())
            {
                DetachedContext detachedContext = new DetachedContext(context);

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
                    AssociatedReference = new AssociatedReference { Id = 1, Name = "Modified Associated Reference 1" },
                };

                await detachedContext.SaveAsync(detachedEntity);

                // THEN the associated reference still exsits:
                Assert.True(context.AssociatedReferences.Any(a => a.Name == "Associated Reference 1"));
            }
        }

        [Fact(DisplayName = "Associated Reference: Remove")]
        public async Task AssociatedReferenceRemove()
        {
            using (TestDbContext context = new TestDbContext())
            {
                DetachedContext detachedContext = new DetachedContext(context);

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

                await detachedContext.SaveAsync(detachedEntity);

                // THEN the associated reference still exsits:
                Assert.True(context.AssociatedReferences.Any(a => a.Name == "Associated Reference 1"));
            }
        }
    }
}