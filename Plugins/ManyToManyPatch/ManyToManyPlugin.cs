using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.ManyToManyPatch
{
    public class ManyToManyPlugin : DetachedPlugin
    {
        public override async Task OnEntitiesLoaded<TEntity>(IList<TEntity> entities)
        {
            if (entities.Any())
            {
                IEntityType entityType = DetachedContext.DbContext.Model.FindEntityType(typeof(TEntity));
                foreach (object entity in entities)
                {
                    foreach (ManyToManyNavigation m2mProperty in entityType.GetManyToManyNavigations())
                    {
                        // query collection for the given many to many (imperformant!!!).
                        MethodInfo loadMethodInfo = LoadCollectionMethodInfo
                                                        .MakeGenericMethod(m2mProperty.End1.EntityType.ClrType,
                                                                           m2mProperty.End2.EntityType.ClrType);

                        Task<object> loadedCollectionTask = (Task<object>)loadMethodInfo.Invoke(this, new[] { entity });
                        object loadedCollection = await loadedCollectionTask;

                        // set the result to the many to many property.
                        m2mProperty.End1.Setter.SetClrValue(entity, loadedCollection);
                    }
                }
            }
        }

        public override Task OnEntityAdded(EntityEntry newEntityEntry, INavigation parentNavigation, EntityEntry parentEntityEntry)
        {
            UpdateManyToManyEntries(newEntityEntry, EntityState.Added);
            return Task.FromResult(0);
        }

        public override Task OnEntityDeleted(EntityEntry persistedEntityEntry, INavigation parentNavigation, EntityEntry parentEntityEntry)
        {
            UpdateManyToManyEntries(persistedEntityEntry, EntityState.Deleted);
            return Task.FromResult(0);
        }

        public override Task OnEntityMerged(EntityEntry persistedEntityEntry, object detachedEntity, INavigation parentNavigation, bool modified, EntityEntry parentEntityEntry)
        {
            foreach (ManyToManyNavigation navigation in persistedEntityEntry.Metadata.GetManyToManyNavigations())
            {
                IEnumerable newCollection = navigation.End1.Getter.GetClrValue(detachedEntity) as IEnumerable;
                IEnumerable dbCollection = navigation.End1.Getter.GetClrValue(persistedEntityEntry.Entity) as IEnumerable;
                Dictionary<string, object> dbTable = navigation.End2.EntityType.CreateHashTable(dbCollection);

                foreach (object newItem in newCollection)
                {
                    string key = navigation.End2.EntityType.GetKeyForHashTable(newItem);
                    if (dbTable.ContainsKey(key))
                        dbTable.Remove(key);
                    else
                        UpdateManyToManyEntry(persistedEntityEntry, navigation, newItem, EntityState.Added);
                }

                foreach (var dbItem in dbTable)
                {
                    UpdateManyToManyEntry(persistedEntityEntry, navigation, dbItem.Value, EntityState.Deleted);
                }
            }

            return Task.FromResult(0);
        }

        void UpdateManyToManyEntries(EntityEntry end1Entry, EntityState state)
        {
            foreach (ManyToManyNavigation navigation in end1Entry.Metadata.GetManyToManyNavigations())
            {
                IEnumerable list = navigation.End1.Getter.GetClrValue(end1Entry.Entity) as IEnumerable;
                if (list != null)
                {
                    foreach (object end2 in list)
                    {
                        UpdateManyToManyEntry(end1Entry, navigation, end2, state);
                    }
                }
            }
        }

        void UpdateManyToManyEntry(EntityEntry end1Entry, ManyToManyNavigation navigation, object item, EntityState state)
        {
            object entity = Activator.CreateInstance(navigation.IntermediateEntityType.ClrType) as IManyToManyEntity;

            Key key1 = navigation.End1.EntityType.FindPrimaryKey();
            Key key2 = navigation.End2.EntityType.FindPrimaryKey();

            EntityEntry entityEntry = DbContext.ChangeTracker.Entries()
                     .Where(ee =>
                     {
                         var m2m = ee as IManyToManyEntity;
                         if (m2m != null)
                         {
                             if (key1.Equal(end1Entry.Entity, m2m.End1) &&
                                 key2.Equal(item, m2m.End2))
                             {
                                 return true;
                             }
                         }
                         return false;
                     }).SingleOrDefault();

            if (entityEntry != null)


            entityEntry.State = state;
        }

        static MethodInfo LoadCollectionMethodInfo = typeof(ManyToManyPlugin).GetTypeInfo().GetMethod(nameof(LoadCollection), BindingFlags.NonPublic | BindingFlags.Instance);

        async Task<object> LoadCollection<TEnd1, TEnd2>(TEnd1 entity)
            where TEnd1 : class
            where TEnd2 : class
        {
            return await DetachedContext.GetBaseQuery<ManyToManyEntity<TEnd1, TEnd2>>()
                            .AsNoTracking()
                            .Where(m2m => m2m.End1 == entity)
                            .Select(m2m => m2m.End2)
                            .ToListAsync();
        }
    }
}