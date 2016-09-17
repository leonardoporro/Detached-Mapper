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
    /// <summary>
    /// Handles detached root entities. A root is an entity with their owned and associated
    /// children that works as a single unit.
    /// </summary>
    public class DetachedContext
    {
        #region Fields

        DbContext _context;
        QueryBuilder _queryBuilder;

        #endregion

        #region Ctor.

        /// <summary>
        /// Initializes a new instance of DetachedContext.
        /// </summary>
        /// <param name="context">The base DbContext instance.</param>
        public DetachedContext(DbContext context)
        {
            _context = context;
            _queryBuilder = new QueryBuilder(context);
        }

        #endregion

        /// <summary>
        /// Returns an IQueryable with the Includes needed to fetch a detached root entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to get.</typeparam>
        /// <returns>An IQueryable with the needed Includes to fetch detached roots.</returns>
        public virtual IQueryable<TEntity> Roots<TEntity>()
            where TEntity : class
        {
            return _queryBuilder.GetRootQuery<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// Loads a detached root entity by its key.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="key">Entity key values.</param>
        /// <returns>A detached root entity with its associated and owned children.</returns>
        public virtual async Task<TEntity> LoadAsync<TEntity>(params object[] key)
            where TEntity : class
        {
            EntityType entityType = _context.Model.FindEntityType(typeof(TEntity)) as EntityType;

            return await _queryBuilder.GetRootQuery<TEntity>()
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(GetFindByKeyExpression<TEntity>(entityType, entityType.FindPrimaryKey(), key));
        }

        /// <summary>
        /// Saves a detached root entity with its associated and owned children.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to save.</typeparam>
        /// <param name="root">The detached root entity to save.</param>
        /// <returns>The saved root entity.</returns>
        public virtual async Task<TEntity> SaveAsync<TEntity>(TEntity root)
            where TEntity : class
        {
            // temporally disabled autodetect changes
            bool autoDetectChanges = _context.ChangeTracker.AutoDetectChangesEnabled;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            EntityType entityType = _context.Model.FindEntityType(typeof(TEntity)) as EntityType;

            // load the persisted entity, with all the includes
            TEntity dbEntity = _queryBuilder.GetRootQuery<TEntity>()
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

        /// <summary>
        /// Deletes a detached root entity with its owned children.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to delete.</typeparam>
        /// <param name="root">The detached root entity to delete.</param>
        public virtual async Task DeleteAsync<TEntity>(TEntity root)
        {
            EntityType entityType = _context.Model.FindEntityType(typeof(TEntity)) as EntityType;
            Delete(entityType, root);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Merges a detached entity with a persisted entity.
        /// </summary>
        /// <param name="entityType">EntityType of the entities being merged.</param>
        /// <param name="newEntity">The detached entity.</param>
        /// <param name="dbEntity">The entity actually persisted in the database.</param>
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

        /// <summary>
        /// Adds a detached entity.
        /// </summary>
        /// <param name="entityType">The EntityType of the entity being added.</param>
        /// <param name="entity">The entity to add.</param>
        protected virtual void Add(EntityType entityType, object entity)
        {
            if (entity == null)
                return;

            var entry = _context.Entry(entity);
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

        /// <summary>
        /// Deletes a database persisted entity.
        /// </summary>
        /// <param name="entityType">The EntityType of the entity being deleted.</param>
        /// <param name="entity">The entity to delete.</param>
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

        /// <summary>
        /// Attaches a detached entity and set its state to Unchaged (for Associations).
        /// </summary>
        /// <param name="entityType">The EntityType being attached.</param>
        /// <param name="entity">The entity to attach.</param>
        protected virtual void Attach(EntityType entityType, object entity)
        {
            EntityEntry entry = _context.Entry(entity);

            _context.Attach(entity);
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

        /// <summary>
        /// Finds an entity in the given collection by its key.
        /// </summary>
        /// <param name="key">Key of the EntityType of the collection elements.</param>
        /// <param name="collection">Collection to find in.</param>
        /// <param name="value">Entity to find.</param>
        protected virtual object FindByKey(Key key, IEnumerable collection, object value)
        {
            foreach (object item in collection)
            {
                if (EqualByKey(key, item, value))
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Gets an expression to query a root entity by its key.
        /// The expression should look like "entity => entity.Key == [instance.Key]" where 
        /// instance.Key is treated as a constant.
        /// It works for multi-value keys.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity to search.</typeparam>
        /// <param name="entityType">EntityType of the entity to search.</param>
        /// <param name="instance">Detached instance containing the value of the key to search by.</param>
        /// <returns>A search by key expression that can be added directly as a parameter of a .Where call.</returns>
        protected virtual Expression<Func<TEntity, bool>> GetFindByKeyExpression<TEntity>(EntityType entityType, TEntity instance)
        {
            Key key = entityType.FindPrimaryKey();
            object[] keyValues = key.Properties.Select(p => p.Getter.GetClrValue(instance)).ToArray();
            return GetFindByKeyExpression<TEntity>(entityType, key, keyValues);
        }

        /// <summary>
        /// Gets an expression to query a root entity by its key.
        /// The expression should look like "entity => entity.Key == [keyValue]" where 
        /// keyValue is the given value for the key.
        /// It works for multi-value keys.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity to search.</typeparam>
        /// <param name="entityType">EntityType of the entity to search.</param>
        /// <param name="key">The EntityType primary key.</param>
        /// <param name="keyValues">The value(s) of the key to search by.</param>
        /// <returns>A search by key expression that can be added directly as a parameter of a .Where call.</returns>
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