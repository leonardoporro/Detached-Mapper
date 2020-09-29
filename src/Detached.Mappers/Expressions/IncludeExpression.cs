using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.Expressions
{
    public class IncludeExpression : ExtendedExpression
    {
        public IncludeExpression(IEnumerable<Expression> expression)
        {
            Expressions = expression;
        }

        public IEnumerable<Expression> Expressions { get; }

        public override Expression Reduce()
        {
            throw new InvalidOperationException($"Include can't be added outside of a Block.");
        }
    }
}