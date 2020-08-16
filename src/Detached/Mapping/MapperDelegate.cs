using Detached.Mapping.Context;

namespace Detached.Mapping
{
    public delegate TTarget MapperDelegate<TSource, TTarget>(TSource source, TTarget target, IMapperContext mapperContext);

    public delegate object MapperDelegate(object source, object target, IMapperContext mapperContext);
}