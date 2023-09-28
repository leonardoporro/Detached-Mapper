using System;

namespace Detached.Mappers.TypeMappers.POCO.Complex
{
    public class ComplexTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        readonly Func<IMapContext, TTarget> _construct;
        readonly Action<TSource, TTarget, IMapContext> _mapMembers;

        public ComplexTypeMapper(
            Func<IMapContext, TTarget> construct,
            Action<TSource, TTarget, IMapContext> mapMembers)
        {
            _construct = construct;
            _mapMembers = mapMembers;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (Equals(source, null))
            {
                return default;
            }
            else if (context.TryGetResult(source, out TTarget trackedTarget))
            {
                return trackedTarget;
            }
            else
            {
                if (Equals(target, null))
                {
                    target = _construct(context);
                }

                context.Push(source, target);

                _mapMembers(source, target, context);

                context.Pop();

                return target;
            }
        }
    }
}
