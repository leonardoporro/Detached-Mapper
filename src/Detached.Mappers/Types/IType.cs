using Detached.Mappers.Annotations;
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
        Collection
    }

    public interface IType
    {
        Type ClrType { get; }

        Type ItemClrType { get; }

        AnnotationCollection Annotations { get; }

        MappingSchema MappingSchema { get; } 

        IEnumerable<string> MemberNames { get; }

        ITypeMember GetMember(string memberName);

        Expression BuildNewExpression(Expression context, Expression discriminator);
    }
}