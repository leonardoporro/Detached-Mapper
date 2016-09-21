using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EntityFrameworkCore.Detached
{
    public class UpdateManager : IUpdateManager
    {
        #region Fields

        DbContext _dbContext;

        #endregion

        #region Ctor.

        public UpdateManager(DbContext dbContext )
        {
            _dbContext = dbContext;
        }

        #endregion

        public virtual void Merge(EntityType entityType, object newEntity, object dbEntity)
        {
            EntityEntry dbEntry = _dbContext.Attach(dbEntity);

            foreach (Property property in entityType.GetProperties())
            {
                if (!(property.IsKeyOrForeignKey() || property.IsShadowProperty))
                {
                    object newValue = property.Getter.GetClrValue(newEntity);
                    object dbValue = property.Getter.GetClrValue(dbEntity);

                    if (!object.Equals(dbValue, newValue))
                    {
                        dbEntry.Property(property.Name).CurrentValue = newValue;
                    }
                }
            }

            foreach (Navigation navigation in entityType.GetNavigations())
            {
                bool owned = navigation.IsOwned();
                bool associated = navigation.IsAssociated();

                if (!(associated || owned))
                    continue;

                EntityType navType = navigation.GetTargetType();
                Key navKey = navType.FindPrimaryKey();

                if (navigation.IsCollection())
                {
                    // create hash table for O(N) merge.
                    Dictionary<string, object> dbTable = navType.CreateHashTable((IEnumerable)navigation.Getter.GetClrValue(dbEntity));
                    IEnumerable newCollection = navigation.CollectionAccessor.GetOrCreate(newEntity) as IEnumerable;
                    // a mutable list to store the result.
                    IList mergedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(navType.ClrType)) as IList;

                    foreach (object newItem in newCollection)
                    {
                        object dbItem;
                        string entityKey = navType.GetKeyForHashTable(newItem);
                        if (dbTable.TryGetValue(entityKey, out dbItem))
                        {
                            // item exists. merge.
                            if (owned)
                                Merge(navType, newItem, dbItem);

                            dbTable.Remove(entityKey);
                            mergedList.Add(dbItem);
                        }
                        else
                        {
                            // item does not exists. add owned or attach associated.
                            if (owned)
                                Add(navType, newItem);
                            else
                                Attach(navType, newItem);

                            mergedList.Add(newItem);
                        }
                    }

                    // the rest of the items in the dbTable should be removed.
                    foreach (var dbItem in dbTable)
                    {
                        Delete(navType, dbItem.Value);
                    }

                    // let EF do the rest of the work.
                    dbEntry.Collection(navigation.Name).CurrentValue = mergedList;
                }
                else
                {
                    // merge reference.
                    object newValue = navigation.Getter.GetClrValue(newEntity);
                    object dbValue = navigation.Getter.GetClrValue(dbEntity);

                    if (EqualByKey(navKey, newValue, dbValue))
                    {
                        // merge owned references and do nothing for associated references.
                        if (owned)
                        {
                            Merge(navType, newValue, dbValue);
                        }
                    }
                    else
                    {
                        if (newValue != null)
                        {
                            if (owned)
                                Add(navType, newValue);
                            else
                                Attach(navType, newValue);
                        }
                        else if (owned)
                        {
                            Delete(navType, dbValue);
                        }
                        // let EF do the rest of the work.
                        dbEntry.Reference(navigation.Name).CurrentValue = newValue;
                    }
                }
            }
        }

        public virtual void Add(EntityType entityType, object entity)
        {
            if (entity == null)
                return;

            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Added;

            foreach (Navigation navigation in entityType.GetNavigations())
            {
                bool owned = navigation.IsOwned();
                bool associated = navigation.IsAssociated();
                EntityType navType = navigation.GetTargetType();

                object navValue = navigation.Getter.GetClrValue(entity);
                if (navValue != null)
                {
                    if (navigation.IsCollection())
                    {
                        foreach (object navItem in (IEnumerable)navValue)
                        {
                            if (associated)
                                Attach(navType, navItem);
                            else if (owned)
                                Add(navType, navItem);
                        }
                    }
                    else
                    {
                        if (associated)
                            Attach(navType, navValue);
                        else if (owned)
                            Add(navType, navValue);
                    }
                }
            }
        }

        public virtual void Delete(EntityType entityType, object entity)
        {
            if (entity == null)
                return;

            var entry = _dbContext.Attach(entity);
            entry.State = EntityState.Deleted;

            foreach (Navigation navigation in entityType.GetNavigations())
            {
                if (navigation.IsOwned()) //recursive deletion for owned properties.
                {
                    object value = navigation.Getter.GetClrValue(entity);
                    if (value != null)
                    {
                        EntityType itemType = navigation.GetTargetType();
                        if (navigation.IsCollection())
                        {
                            foreach (object item in (IEnumerable)value)
                            {
                                Delete(itemType, item); //delete collection item.
                            }
                        }
                        else
                        {
                            Delete(itemType, value); //delete reference.
                        }
                    }
                }
            }
        }

        public virtual void Attach(EntityType entityType, object entity)
        {
            EntityEntry entry = _dbContext.Entry(entity);

            _dbContext.Attach(entity);
            entry.State = EntityState.Unchanged;
        }

        /// <summary>
        /// Compares two in-memory entities by its key.
        /// </summary>
        /// <param name="key">Key instance of the entities to compare.</param>
        /// <param name="a">Entity to compare.</param>
        /// <param name="b">Entity to compare.</param>
        /// <returns></returns>
        protected virtual bool EqualByKey(Key key, object a, object b)
        {
            bool equal = a != null && b != null;
            if (equal)
            {
                foreach (Property property in key.Properties)
                {
                    object aValue = property.Getter.GetClrValue(a);
                    object bValue = property.Getter.GetClrValue(b);
                    if (!object.Equals(aValue, bValue))
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }
    }
}
