using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IKeyServices<TEntity> : IKeyServices
    {
        Expression<Func<TEntity, bool>> CreateEqualityExpression(object[] keyValues);
    }

    public interface IKeyServices
    {
        bool Equal(object entityA, object entityB);

        object[] GetValues(object entity);

        IKey GetKey();

        Dictionary<object[], object> CreateTable(IEnumerable entities);
    }
}