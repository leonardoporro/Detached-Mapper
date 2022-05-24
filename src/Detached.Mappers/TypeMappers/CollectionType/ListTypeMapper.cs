using Detached.Mappers.Context;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.CollectionType
{
    public class ListTypeMapper<TSource, TTarget> : ITypeMapper<List<TSource>, List<TTarget>>
    {
        readonly LazyTypeMapper<TSource, TTarget> _itemMapper;

        public ListTypeMapper(LazyTypeMapper<TSource, TTarget> itemMapper)
        {
            _itemMapper = itemMapper;
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((List<TSource>)source, (List<TTarget>)target, mapperContext);
        }

        public List<TTarget> Map(List<TSource> source, List<TTarget> target, IMapperContext mapperContext)
        {
            List<TTarget> result = null;

            if (source != null)
            {
                result = new List<TTarget>(source.Count);

                ITypeMapper<TSource, TTarget> itemMapper = _itemMapper.Value;

                for (int i = 0; i < source.Count; i++)
                {
                    TSource sourceValue = source[i];
                    TTarget targetValue = default;
                    if (target != null)
                    {
                        targetValue = target[i];
                    }

                    result.Add(itemMapper.Map(sourceValue, targetValue, mapperContext));
                }
            }

            return result;
        }
    }
}
