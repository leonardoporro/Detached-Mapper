using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeOptions
{
    public enum TypeKind
    {
        Primitive,
        Complex,
        Collection,
        Entity,
        Nullable,
        Boxed
    }

    public interface ITypeOptions
    {
        Dictionary<string, object> Annotations { get; }

        bool IsCollection { get; }

        bool IsEntity { get; }

        bool IsFragment { get; }

        bool IsComplex { get; }

        bool IsPrimitive { get; }

        bool IsNullable { get; }

        bool IsAbstract { get; }

        Type ItemType { get; }

        IEnumerable<string> MemberNames { get; }

        string DiscriminatorName { get; }

        Dictionary<object, Type> DiscriminatorValues { get; }

        Type ClrType { get; }

        Expression BuildNewExpression(Expression context, Expression discriminator);

        Expression BuildIsSetExpression(Expression instance, Expression context, string memberName);

        IMemberOptions GetMember(string memberName);
    }
}