using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypeMappers.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.TypeMappers
{
    public abstract class EntityTypeMapper<TSource, TTarget, TKey> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        protected EntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers,
            Action<TSource, TTarget, IMapContext> mapNoKeyMembers)
        {
            Construct = construct;
            GetSourceKey = getSourceKey;
            GetTargetKey = getTargetKey;
            MapKeyMembers = mapKeyMembers;
            MapMembers = mapNoKeyMembers;
        }

        protected Action<TSource, TTarget, IMapContext> MapKeyMembers { get; }

        protected Action<TSource, TTarget, IMapContext> MapMembers { get; }

        protected Func<IMapContext, TTarget> Construct { get; }

        protected Func<TTarget, IMapContext, TKey> GetTargetKey { get; }

        protected Func<TSource, IMapContext, TKey> GetSourceKey { get; }

        protected virtual TTarget Create(TSource source, TKey key, IMapContext mapContext, EntityRef entityRef)
        {
            var entry = GetEntry(source, key, mapContext);

            if (!key.IsEmpty && mapContext.Parameters.ExistingCompositionBehavior == ExistingCompositionBehavior.Associate)
            {
                entry.Reload();
            }

            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }

            TTarget target = entry.Entity;
            mapContext.Push(entityRef, target);

            MapMembers(source, target, mapContext);

            mapContext.Pop();

            return target;
        }

        protected virtual TTarget Merge(TSource source, TTarget target, TKey key, IMapContext mapContext, EntityRef entityRef)
        {
            var entry = GetEntry(source, key, mapContext);
            target = entry.Entity;

            mapContext.Push(entityRef, target);

            MapMembers(source, target, mapContext);

            //if (Options.ConcurrencyTokens.TryGetValue(entry.Metadata.Name, out string tokenName))
            //{
            //    PropertyEntry tokenProperty = entry.Property(tokenName);
            //    tokenProperty.OriginalValue = tokenProperty.CurrentValue;
            //}

            mapContext.Pop();

            return target;
        }

        protected virtual TTarget Replace(TSource source, IMapContext context, TKey key, EntityRef entityRef)
        {
            TTarget target = Create(source, key, context, entityRef);
            Delete(source, target, key, context);

            return target;
        }

        protected virtual TTarget Delete(TSource source, TTarget target, TKey key, IMapContext mapContext)
        {
            var entry = GetEntry(source, key, mapContext);
            entry.State = EntityState.Deleted;

            return null;
        }

        protected virtual TTarget Attach(TSource source, TKey key, IMapContext mapContext)
        {
            var entry = GetEntry(source, key, mapContext);

            TTarget target = entry.Entity;

            if (mapContext.Parameters.MissingAggregationBehavior == MissingAggregationBehavior.Create)
            {
                entry.Reload();
                if (entry.State == EntityState.Detached)
                {
                    entry.State = EntityState.Added;
                }
            }
            else
            {
                MapMembers(source, target, mapContext); // Only primitives, like Name, Description, etc.
                entry.State = EntityState.Unchanged;
            }

            return target;
        }

        public EntityEntry<TTarget> GetEntry(TSource source, TKey key, IMapContext mapContext)
        {
            var dbContext = ((EntityMapContext)mapContext).DbContext;

            EntityEntry<TTarget> entry;

            var stateManager = dbContext.GetService<IStateManager>();
            var entityType = dbContext.Model.FindEntityType(typeof(TTarget));
            var keyType = entityType.FindPrimaryKey();
            var internalEntry = stateManager.TryGetEntry(keyType, key.ToObject());

            if (internalEntry != null)
            {
                entry = new EntityEntry<TTarget>(internalEntry);
            }
            else
            {
                TTarget target = Construct(mapContext);
                MapKeyMembers(source, target, mapContext);

                entry = dbContext.Entry(target);
            }

            return entry;
        }
    }
}