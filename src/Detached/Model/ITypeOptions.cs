using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Model
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
        
        Type Type { get; }

        Expression Construct(Expression context);

        IMemberOptions GetMember(string memberName);
    }
}