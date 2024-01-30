using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeBinders.Binders
{
    public class ForeignKeyTypeBinder : ITypeBinder
    {
        public bool CanBind(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsComplex() && typePair.TargetType.IsPrimitive();
        }

        public Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr)
        {
            var keyMember = typePair.SourceType.GetKeyMember();
            var keyPropertyInfo = keyMember.GetPropertyInfo();

            Expression memberSourceExpr = keyMember.BuildGetExpression(sourceExpr, null);
 

            return memberSourceExpr;
        }
    }
}
