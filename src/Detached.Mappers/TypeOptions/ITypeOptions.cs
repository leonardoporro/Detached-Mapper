using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeOptions
{
    public interface ITypeOptions
    {
        Dictionary<string, object> Annotations { get; }

        bool IsCollection { get; }

        bool IsEntity { get; }

        bool IsFragment { get; }

        bool IsComplexType { get; }

        bool IsValue { get; }

        Type ItemType { get; }

        IEnumerable<string> MemberNames { get; }

        string DiscriminatorName { get; }

        Dictionary<object, Type> DiscriminatorValues { get; }

        Type Type { get; }

        Expression Construct(Expression context, Expression discriminator);

        IMemberOptions GetMember(string memberName);
    }
}