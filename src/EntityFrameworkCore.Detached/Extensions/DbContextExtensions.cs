using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public static class DbContextExtensions
    {
        //public static ReferenceNavigationBuilder<TEntity, TProperty> OwnsOne<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, IEnumerable<TProperty>>> navigationExpression)
        //    where TEntity : class
        //{
        //    builder.HasOne(navigationExpression);

        //    string name = ((MemberExpression)navigationExpression.Body).Member.Name;
        //    Navigation nav = builder.Metadata.GetNavigations().Where(e => e.Name == name).FirstOrDefault() as Navigation;
        //    nav.AddAnnotation("Owned", true);

        //    return builder;
        //}
    }
}
