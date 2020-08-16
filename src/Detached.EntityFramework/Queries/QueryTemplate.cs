using Detached.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace Detached.EntityFramework.Queries
{
    public class QueryTemplate<TSource, TTarget>
        where TTarget : class
        where TSource : class
    {
        public ConstantExpression SourceConstant { get; set; }

        public Expression<Func<TTarget, bool>> FilterExpression { get; set; }

        public Expression<Func<TTarget, TSource>> ProjectExpression { get; set; }

        public List<string> Includes { get; } = new List<string>();

        public IQueryable<TTarget> Render(IQueryable<TTarget> queryable, TSource source)
        {
            ReplaceExpressionVisitor visitor = new ReplaceExpressionVisitor(
               new Dictionary<Expression, Expression>
               {
                    { SourceConstant, Constant(source) }
               }
            );
            var filter = (Expression<Func<TTarget, bool>>)visitor.Visit(FilterExpression);

            queryable = queryable.Where(filter);

            foreach (string include in Includes)
            {
                queryable.Include(include);
            }

            return queryable;
        }
    }
}