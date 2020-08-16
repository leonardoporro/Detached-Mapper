using System;
using System.Linq.Expressions;

namespace Detached.Expressions
{
    public class ReturnTypeExpression : ExtendedExpression
    {
        public ReturnTypeExpression(Type type, out LabelTarget returnTarget)
        {
            returnTarget = Expression.Label(type, "$return");
            LabelTarget = returnTarget;
            Type = type;
        }

        public LabelTarget LabelTarget { get; }

        public override Type Type { get; }
    }
}