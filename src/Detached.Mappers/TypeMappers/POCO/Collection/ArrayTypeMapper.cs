using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.POCO.Collection
{
    public class ArrayTypeMapper<TSource, TSourceItem, TTarget, TTargetItem> : TypeMapper<TSource, TTarget>
    {
        public override TTarget Map(TSource source, TTarget target, IMapperContext mapperContext)
        {
            throw new System.NotImplementedException();
        }
    }
}