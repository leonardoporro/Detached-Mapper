using Detached.RuntimeTypes.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public class LoaderQuery<TSource, TTarget> : ILoaderQuery
        where TSource : class
        where TTarget : class
    { 
        public LoaderQuery(ConstantExpression sourceConstant, Expression<Func<TTarget, bool>> filter, List<string> includes)
        {
            SourceConstant = sourceConstant;
            FilterExpression = filter;
            Includes = includes;
        }

        public ConstantExpression SourceConstant { get; set; }

        public Expression<Func<TTarget, bool>> FilterExpression { get; set; }

        public List<string> Includes { get; set; }

        public object Load(DbContext dbContext, object entityOrDto)
        {
            return GetQuery(dbContext.Set<TTarget>(), (TSource)entityOrDto).FirstOrDefault();
        }

        public async Task<object> LoadAsync(DbContext dbContext, object entityOrDto)
        {
            return await GetQuery(dbContext.Set<TTarget>(), (TSource)entityOrDto).FirstOrDefaultAsync();
        }

        IQueryable<TTarget> GetQuery(IQueryable<TTarget> queryable, TSource source)
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
                queryable = queryable.Include(include);
            }

            return queryable;
        } 
    }
}