using System;
using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.Class.Primitive
{
    public class PrimitiveTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (Equals(source, null))
                return default;
            else
                return (TTarget)Convert.ChangeType(source, typeof(TTarget));
        }
    }
}
