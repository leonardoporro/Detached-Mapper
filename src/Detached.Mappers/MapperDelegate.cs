using Detached.Mappers.Context;

namespace Detached.Mappers
{
    public delegate TTarget MapperDelegate<TSource, TTarget>(TSource source, TTarget target, IMapperContext mapperContext);

    public delegate object MapperDelegate(object source, object target, IMapperContext mapperContext);
}