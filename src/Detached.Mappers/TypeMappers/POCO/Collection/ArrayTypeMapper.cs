namespace Detached.Mappers.TypeMappers.POCO.Collection
{
    public class ArrayTypeMapper<TSource, TSourceItem, TTarget, TTargetItem> : TypeMapper<TSource, TTarget>
    {
        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}