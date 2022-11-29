using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeOptions.Dictionary
{
    public class DictionaryTypeOptions : ITypeOptions
    {
        public Type ClrType => typeof(Dictionary<string, object>);
        
        public Type ItemClrType => null;

        public MappingStrategy MappingStrategy => MappingStrategy.Complex;

        public Dictionary<string, object> Annotations { get; set; }
 
        public IEnumerable<string> MemberNames => new string[0];

        public bool IsAbstract => false;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            return New(typeof(Dictionary<string, object>));
        }

        public IMemberOptions GetMember(string memberName)
        {
            return new DictionaryMemberOptions(memberName);
        }
    }
}