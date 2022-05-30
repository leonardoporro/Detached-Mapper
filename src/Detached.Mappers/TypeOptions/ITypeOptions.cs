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
        Nullable
    }

    public interface ITypeOptions
    {
        Type ClrType { get; }        
        
        Type ItemClrType { get; }

        Dictionary<string, object> Annotations { get; }

        public TypeKind Kind { get; }

        IEnumerable<string> MemberNames { get; }

        string DiscriminatorName { get; }

        Dictionary<object, Type> DiscriminatorValues { get; }
 
        IMemberOptions GetMember(string memberName);

        Expression BuildNewExpression(Expression context, Expression discriminator);

        Expression BuildIsSetExpression(Expression instance, Expression context, string memberName);
    }
}