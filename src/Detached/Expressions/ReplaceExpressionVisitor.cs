using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Expressions
{
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        readonly Dictionary<Expression, Expression> _replacements;

        public ReplaceExpressionVisitor(Dictionary<Expression, Expression> replacements)
        {
            _replacements = replacements;
        }

        public override Expression Visit(Expression node)
        {
            if (node == null) return null;

            if (_replacements.TryGetValue(node, out Expression replacement))
                return replacement;
            else
                return base.Visit(node);
        }
    }
}