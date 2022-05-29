using System;

namespace Detached.Mappers.TypeMappers.POCO.Primitive
{
    public class PrimitiveTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (Equals(source, null))
                return default(TTarget);
            else
                return (TTarget)Convert.ChangeType(source, typeof(TTarget));
        }
    }
}
