using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.POCO.Collection
{
    public class CollectionTypeMapper<TSource, TSourceItem, TTarget, TTargetItem> : TypeMapper<TSource, TTarget>
        where TSource : class, IEnumerable<TSourceItem>
        where TTarget : class, ICollection<TTargetItem>
    {
        readonly LazyTypeMapper<TSourceItem, TTargetItem> _itemMapper;
        readonly Func<TTarget> _construct;

        public CollectionTypeMapper(
            Func<TTarget> construct,
            LazyTypeMapper<TSourceItem, TTargetItem> itemMapper)
        {
            _construct = construct;
            _itemMapper = itemMapper;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            TTarget result = null;

            if (source != null)
            {
                result = Activator.CreateInstance<TTarget>();

                ITypeMapper<TSourceItem, TTargetItem> itemMapper = _itemMapper.Value;

                foreach (TSourceItem sourceItem in source)
                {
                    result.Add(itemMapper.Map(sourceItem, default, context));
                }
            }

            return result;
        }
    }
}
