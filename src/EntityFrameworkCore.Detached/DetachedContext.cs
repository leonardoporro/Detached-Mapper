using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public class DetachedContext
    {
        #region Fields

        DbContext _context;
        QueryBuilder _queryBuilder;

        #endregion

        #region Ctor.

        public DetachedContext(DbContext context)
        {
            _context = context;
            _queryBuilder = new QueryBuilder(context);
        }

        #endregion

        public virtual IQueryable<TEntity> Roots<TEntity>()
            where TEntity : class
        {
            return _queryBuilder.GetRootQuery<TEntity>().AsNoTracking();
        }

        public virtual async Task<TEntity> Load<TEntity>(params object[] key)
            where TEntity : class
        {
            EntityType entityType = _context.Model.FindEntityType(typeof(TEntity)) as EntityType;

            return await _queryBuilder.GetRootQuery<TEntity>()
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(GetFindByKeyExpression<TEntity>(entityType, entityType.FindPrimaryKey(), key));
        }

        public virtual async Task<TRoot> Save<TRoot>(TRoot root)
            where TRoot : class
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _context.ChangeTracker.AutoDetectChangesEnabled;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            EntityType entityType = _context.Model.FindEntityType(typeof(TRoot)) as EntityType;

            // load the persisted entity, with all the includes
            TRoot dbEntity = _queryBuilder.GetRootQuery<TRoot>()
                                .AsTracking()
                                .SingleOrDefault(GetFindByKeyExpression(entityType, root));

            if (dbEntity == null)
                Add(entityType, root); // entity does not exist.
            else
                Merge(entityType, root, dbEntity); // entity exists.

            // re-enable autodetect changes.
            await _context.SaveChangesAsync();
            _context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

            return dbEntity;
        }

        public virtual async Task Delete<TRoot>(TRoot root)
        {
            EntityType entityType = _context.Model.FindEntityType(typeof(TRoot)) as EntityType;
            Delete(entityType, root);
            await _context.SaveChangesAsync();
        }

        protected virtual void Merge(EntityType entityType, object newEntity, object dbEntity)
        {
            EntityEntry dbEntry = _context.Attach(dbEntity);

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
                    //merge collection.
                    IEnumerable dbCollection = navigation.CollectionAccessor.GetOrCreate(dbEntity) as IEnumerable;
                    IEnumerable newCollection = navigation.CollectionAccessor.GetOrCreate(newEntity) as IEnumerable;

                    IList collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(navType.ClrType));

                    //add/merge items that don't exist in database.
                    foreach (object newItem in newCollection)
                    {
                        object dbItem = FindByKey(navKey, dbCollection, newItem);
                        if (dbItem != null)
                        {
                            if (owned)
                                Merge(navType, newItem, dbItem);

                            collection.Add(dbItem);
                        }
                        else
                        {
                            if (newItem != null)
                            {
                                if (owned)
                                    Add(navType, newItem);
                                else
                                    Attach(navType, newItem);
                            }
                            collection.Add(newItem);
                        }
                    }

                    foreach (object dbItem in dbCollection)
                    {
                        if (FindByKey(navKey, newCollection, dbItem) == null)
                            Delete(navType, dbItem);
                    }

                    dbEntry.Collection(navigation.Name).CurrentValue = collection;
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

        protected virtual void Add(EntityType entityType, object entity)
        {
            if (entity == null)
                return;

            var entry = _context.Attach(entity);
            entry.State = EntityState.Added;

            foreach (Navigation navigation in entityType.GetNavigations())
            {
                if (navigation.IsOwned()) //recursive add for owned properties.
                {
                    object value = navigation.Getter.GetClrValue(entity);
                    if (value != null)
                    {
                        EntityType itemType = navigation.GetTargetType();
                        if (navigation.IsCollection())
                        {
                            foreach (object item in (IEnumerable)value)
                            {
                                Add(itemType, item); //add collection item.
                            }
                        }
                        else
                        {
                            Add(itemType, value); //add reference.
                        }
                    }
                }
            }
        }

        protected virtual void Delete(EntityType entityType, object entity)
        {
            if (entity == null)
                return;

            var entry = _context.Attach(entity);
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

        protected virtual void Attach(EntityType entityType, object entity)
        {
            EntityEntry entry = _context.Entry(entity);

            _context.Attach(entity);
            entry.State = EntityState.Unchanged;
        }

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

        protected virtual object FindByKey(Key key, IEnumerable collection, object value)
        {
            foreach (object item in collection)
            {
                if (EqualByKey(key, item, value))
                    return item;
            }
            return null;
        }

        protected virtual Expression<Func<TEntity, bool>> GetFindByKeyExpression<TEntity>(EntityType entityType, TEntity instance)
        {
            Key key = entityType.FindPrimaryKey();
            object[] keyValues = key.Properties.Select(p => p.Getter.GetClrValue(instance)).ToArray();
            return GetFindByKeyExpression<TEntity>(entityType, key, keyValues);
        }

        protected virtual Expression<Func<TEntity, bool>> GetFindByKeyExpression<TEntity>(EntityType entityType, Key key, object[] keyValues)
        {
            ParameterExpression param = Expression.Parameter(entityType.ClrType, entityType.ClrType.Name.ToLower());

            Func<int, Expression> buildCompare = i =>
                Expression.Equal(Expression.Property(param, key.Properties[i].PropertyInfo),
                                 Expression.Constant(keyValues[i]));

            Expression findExpr = buildCompare(0);

            for (int i = 1; i < key.Properties.Count; i++)
            {
                findExpr = Expression.AndAlso(findExpr, buildCompare(i));
            }

            return Expression.Lambda<Func<TEntity, bool>>(findExpr, param);
        }
    }
}