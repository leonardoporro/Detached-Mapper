using Detached.Mappers.Annotations;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.Types
{
    public interface ITypeMember
    {
        AnnotationCollection Annotations { get; }

        string Name { get; }

        Type ClrType { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        bool CanTryGet { get; }

        Expression BuildGetExpression(Expression instance, Expression context);

        Expression BuildTryGetExpression(Expression instance, Expression context, Expression outVar);

        Expression BuildSetExpression(Expression instance, Expression value, Expression context);
    }
}