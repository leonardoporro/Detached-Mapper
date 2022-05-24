using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeOptions
{
    public interface ITypeOptions
    {
        Dictionary<string, object> Annotations { get; }

        bool IsCollectionType { get; }

        bool IsEntityType { get; }

        bool IsFragment { get; }

        bool IsComplexType { get; }

        bool IsPrimitiveType { get; }

        Type ItemType { get; }

        IEnumerable<string> MemberNames { get; }

        string DiscriminatorName { get; }

        Dictionary<object, Type> DiscriminatorValues { get; }

        Type Type { get; }

        Expression Construct(Expression context, Expression discriminator);

        IMemberOptions GetMember(string memberName);
    }
}