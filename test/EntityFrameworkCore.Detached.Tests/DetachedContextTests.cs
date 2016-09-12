using EntityFrameworkCore.Detached.Conventions;
using EntityFrameworkCore.Detached.Tests.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkCore.Detached.Tests
{
    public class DetachedContextTests
    {
        [Fact(DisplayName = "Owned Collection: Add Item")]
        public void OwnedCollectionAddItem()
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

                detachedContext.UpdateRoot(detachedEntity);
                detachedContext.SaveChanges();

                // THEN the items is added to the the database.
                Entity persistedEntity = detachedContext.Roots<Entity>().Single(r => r.Id == 1);
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item A"));
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item B"));
            }
        }

        [Fact(DisplayName = "Owned Collection: Remove Item")]
        public void OwnedCollectionRemoveItem()
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

                detachedContext.UpdateRoot(detachedEntity);
                detachedContext.SaveChanges();

                // THEN the items is added to the the database.
                Entity persistedEntity = detachedContext.Roots<Entity>().Single(r => r.Id == 1);
                Assert.False(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item A"));
                Assert.True(persistedEntity.OwnedList.Any(s => s.Name == "Owned Item B"));
            }
        }

        [Fact(DisplayName = "Owned Reference: Set")]
        public void OwnedRefereceSet()
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
                detachedContext.UpdateRoot(detachedEntity);
                detachedContext.SaveChanges();

                // THEN the owned reference is replaced:
                Assert.False(context.OwnedReferences.Any(o => o.Name == "Owned Reference 1"));
            }
        }

        [Fact(DisplayName = "Owned Reference: Remove")]
        public void OwnedReferenceRemove()
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
                detachedContext.UpdateRoot(detachedEntity);
                detachedContext.SaveChanges();

                // THEN the owned reference is removed:
                Assert.False(context.OwnedReferences.Any(o => o.Name == "Owned Reference 1"));
            }
        }

        [Fact(DisplayName = "Associated Reference: Set")]
        public void AssociatedReferenceSet()
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
                detachedContext.UpdateRoot(detachedEntity);
                detachedContext.SaveChanges();

                // THEN the associated reference still exsits:
                Assert.True(context.AssociatedReferences.Any(a => a.Name == "Associated Reference 1"));
            }
        }

        [Fact(DisplayName = "Associated Reference: Remove")]
        public void AssociatedReferenceRemove()
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
                detachedContext.UpdateRoot(detachedEntity);
                detachedContext.SaveChanges();

                // THEN the associated reference still exsits:
                Assert.True(context.AssociatedReferences.Any(a => a.Name == "Associated Reference 1"));
            }
        }
    }
}
