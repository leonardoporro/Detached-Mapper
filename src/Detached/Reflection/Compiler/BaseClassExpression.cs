using System;
using System.Linq.Expressions;

namespace Detached.Reflection.Compiler
{
    public class BaseClassExpression : Expression
    {
        readonly Expression _thisExpr;

        public BaseClassExpression(ParameterExpression thisExpr)
        {
            _thisExpr = thisExpr; 
        }

        public override Type Type => _thisExpr.Type;

        public override ExpressionType NodeType => ExpressionType.Extension;

        public override bool CanReduce => true;

        public override Expression Reduce()
        {
            return _thisExpr;
        }
    }
}