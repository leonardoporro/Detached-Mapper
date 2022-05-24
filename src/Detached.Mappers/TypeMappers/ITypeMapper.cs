using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers
{
    public interface ITypeMapper
    {
        object Map(object source, object target, IMapperContext mapperContext);
    }

    public interface ITypeMapper<TSource, TTarget> : ITypeMapper
    {
        TTarget Map(TSource source, TTarget target, IMapperContext mapperContext);
    }
}