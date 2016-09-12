using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public static class EntityTypeExtensions
    {
        public static Expression<Func<TEntity, bool>> GetFindByKeyExpression<TEntity>(this EntityType entityType, TEntity instance)
        {
            Key key = entityType.FindPrimaryKey();

            ParameterExpression param = Expression.Parameter(entityType.ClrType, entityType.ClrType.Name.ToLower());

            Func<int, Expression> buildCompare = i =>
                Expression.Equal(Expression.Property(param, key.Properties[i].PropertyInfo),
                                 Expression.Constant(key.Properties[i].Getter.GetClrValue(instance)));

            Expression findExpr = buildCompare(0);

            for (int i = 1; i < key.Properties.Count; i++)
            {
                findExpr = Expression.AndAlso(findExpr, buildCompare(i));
            }

            return Expression.Lambda<Func<TEntity, bool>>(findExpr, param);
        }
    }
}
