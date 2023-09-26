using Detached.Mappers.TypePairs;
using Detached.Mappers.Types.Class;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeBinders.Binders
{
    public class PrimitiveTypeBinder : ITypeBinder
    {
        public bool CanBind(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsPrimitive() && typePair.TargetType.IsPrimitive();
        }

        public Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr)
        {
            return sourceExpr;
        }
    }
}
