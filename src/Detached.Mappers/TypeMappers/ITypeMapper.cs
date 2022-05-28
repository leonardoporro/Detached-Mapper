using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapper
    {
        object Map(object source, object target, IMapContext context);
    }

    public interface ITypeMapper<TSource, TTarget> : ITypeMapper
    {
        TTarget Map(TSource source, TTarget target, IMapContext context);
    }
}