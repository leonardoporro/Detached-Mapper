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
            return GetQuery(dbContext.Set<TTarget>(), (TSource)entityOrDto)
                    .FirstOrDefault();
        }

        public IEnumerable<object> Load(DbContext dbContext, IEnumerable<object> entitiesOrDtos)
        {
            return GetQuery(dbContext.Set<TTarget>(), (IEnumerable<TSource>)entitiesOrDtos)
                    .ToList();
        }

        public async Task<object> LoadAsync(DbContext dbContext, object entityOrDto)
        {
            return await GetQuery(dbContext.Set<TTarget>(), (TSource)entityOrDto)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<object>> LoadAsync(DbContext dbContext, IEnumerable<object> entitiesOrDtos)
        {
            return await GetQuery(dbContext.Set<TTarget>(), (IEnumerable<TSource>)entitiesOrDtos)
                   .ToListAsync();
        }

        IQueryable<TTarget> GetQuery(IQueryable<TTarget> queryable, TSource source)
        {
            var visitor = new ReplaceExpressionVisitor();
            visitor.Node = SourceConstant;
            visitor.Replacement = Constant(source);
            var filter = (Expression<Func<TTarget, bool>>)visitor.Visit(FilterExpression);

            queryable = queryable.Where(filter);

            foreach (string include in Includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable;
        }

        IQueryable<TTarget> GetQuery(IQueryable<TTarget> queryable, IEnumerable<TSource> sources)
        {
            var visitor = new ReplaceExpressionVisitor();
            visitor.Node = SourceConstant;

            Expression filter = null;
            foreach(var source in sources)
            {
                visitor.Replacement = Constant(source);

                var replaced = visitor.Visit(FilterExpression.Body);
                if (filter == null)
                    filter = replaced;
                else
                    filter = OrElse(filter, replaced);
            }

            var filterFn = Lambda<Func<TTarget, bool>>(filter, FilterExpression.Parameters);

            queryable = queryable.Where(filterFn);

            foreach (string include in Includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable;
        }

        public class ReplaceExpressionVisitor : ExpressionVisitor
        {
            public Expression Node { get; set; }

            public Expression Replacement { get; set; }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                {
                    return null;
                }

                if (Node == node)
                {
                    return Replacement;
                }

                return base.Visit(node);
            }
        }
    }
}