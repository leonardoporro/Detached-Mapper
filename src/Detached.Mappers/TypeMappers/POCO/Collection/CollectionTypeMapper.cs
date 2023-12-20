using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.POCO.Collection
{
    public class CollectionTypeMapper<TSource, TSourceItem, TTarget, TTargetItem> : TypeMapper<TSource, TTarget>
        where TSource : class, IEnumerable<TSourceItem>
        where TTarget : class, ICollection<TTargetItem>
    {
        readonly ITypeMapper<TSourceItem, TTargetItem> _itemMapper;
        readonly Func<IMapContext, TTarget> _construct;

        public CollectionTypeMapper(
            Func<IMapContext, TTarget> construct,
            ITypeMapper<TSourceItem, TTargetItem> itemMapper)
        {
            _construct = construct;
            _itemMapper = itemMapper;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            TTarget result = null;

            if (source != null)
            {
                result = _construct(context);

                foreach (TSourceItem sourceItem in source)
                {
                    result.Add(_itemMapper.Map(sourceItem, default, context));
                }
            }

            return result;
        }
    }
}