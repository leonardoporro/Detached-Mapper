namespace Detached.Mappers.TypeMappers
{
    public abstract class TypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        public abstract TTarget Map(TSource source, TTarget target, IMapContext mapContext);

        public object Map(object source, object target, IMapContext mapContext)
        {
            return Map((TSource)source, (TTarget)target, mapContext);
        }
    }
}
