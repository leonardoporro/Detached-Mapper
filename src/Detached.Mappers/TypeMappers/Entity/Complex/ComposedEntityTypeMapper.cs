﻿using System;
using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class ComposedEntityTypeMapper<TSource, TTarget, TKey> : EntityTypeMapper<TSource, TTarget, TKey>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        public ComposedEntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers,
            Action<TSource, TTarget, IMapContext> mapNoKeyMembers)
            : base(construct, getSourceKey, getTargetKey, mapKeyMembers, mapNoKeyMembers)
        {
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (source == null)
            {
                if (target != null)
                {
                    TKey targetKey = GetTargetKey(target, context);
                    target = Delete(target, targetKey, context);
                }
            }
            else
            {
                TKey sourceKey = GetSourceKey(source, context);
                EntityRef entityRef = new EntityRef(sourceKey, typeof(TTarget));

                if (context.TryGetResult(entityRef, out TTarget parent))
                {
                    target = parent;
                }
                else if (target == null)
                {
                    target = Create(source, sourceKey, context, entityRef);
                }
                else
                {
                    TKey targetKey = GetTargetKey(target, context);

                    if (Equals(sourceKey, targetKey))
                    {
                        target = Merge(source, target, targetKey, context, entityRef);
                    }
                    else
                    {
                        target = Replace(source, context, sourceKey, entityRef, targetKey);
                    }
                }
            }

            return target;
        }
    }
}