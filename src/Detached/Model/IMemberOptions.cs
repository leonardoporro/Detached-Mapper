using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Model
{
    public interface IMemberOptions
    {
        Dictionary<string, object> Annotations { get; }
        
        bool Ignored { get; }
        
        bool IsKey { get; }
        
        string Name { get; }
        
        bool Owned { get; }
        
        Type Type { get; }

        bool CanRead { get; }

        Expression GetValue(Expression instance, Expression context);
        
        bool CanWrite { get; }

        Expression SetValue(Expression instance, Expression value, Expression context);
    }
}