using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeBinders.Binders
{
    public class NullableTypeBinder : ITypeBinder
    {
        public bool CanBind(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsNullable() || typePair.TargetType.IsNullable();
        }

        public Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr)
        {
            Expression result;

            if (typePair.SourceType.IsNullable())
            {
                if (typePair.TargetType.IsNullable())
                {
                    result = sourceExpr;
                }
                else
                {
                    result = Property(sourceExpr, "Value");
                }
            }
            else
            {
                result = sourceExpr;
            }

            return result;
        }
    }
}
