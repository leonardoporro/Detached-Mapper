using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.Types
{
    public enum MappingSchema
    {
        None,
        Primitive,
        Complex,
        Collection,
        Nullable
    }

    public interface IType
    {
        Type ClrType { get; }

        Type ItemClrType { get; }

        Dictionary<string, object> Annotations { get; }

        MappingSchema MappingSchema { get; }

        bool IsAbstract { get; set; }

        IEnumerable<string> MemberNames { get; }

        ITypeMember GetMember(string memberName);

        Expression BuildNewExpression(Expression context, Expression discriminator);
    }
}