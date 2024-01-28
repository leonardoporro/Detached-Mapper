using Detached.Mappers.TypeMappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Detached.Mappers.EntityFramework.TypeMappers
{
    public class KeyToEntityTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
        where TTarget : class
    {
        readonly Func<IMapContext, TTarget> _construct;
        readonly Action<TSource, TTarget, IMapContext> _setKey;

        public KeyToEntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Action<TSource, TTarget, IMapContext> set)
        {
            _construct = construct;
            _setKey = set;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext mapContext)
        {
            var dbContext = ((EntityMapContext)mapContext).DbContext;

            var entry = GetExistingEntry(source, dbContext);

            if (entry == null)
            {
                target = _construct(mapContext);
               
                _setKey(source, target, mapContext);

                entry = dbContext.Entry(target);
                entry.State = EntityState.Unchanged;
            }

            return entry.Entity;
        }

        IKey _keyType;

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "I still need to use internal things to make this library work.")]
        public EntityEntry<TTarget> GetExistingEntry(TSource source, DbContext dbContext)
        {
            if (_keyType == null)
            {
                var entityType = dbContext.Model.FindEntityType(typeof(TTarget));
                _keyType = entityType.FindPrimaryKey();
            }

            var stateManager = dbContext.GetService<IStateManager>();
            var internalEntry = stateManager.TryGetEntry(_keyType, new object[] { source });

            if (internalEntry != null)
                return new EntityEntry<TTarget>(internalEntry);
            else
                return null;
        }
    }
}