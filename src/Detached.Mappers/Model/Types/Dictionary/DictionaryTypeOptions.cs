using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.Model.Types.Dictionary
{
    public class DictionaryTypeOptions : ITypeOptions
    {
        public Dictionary<string, object> Annotations { get; set; }

        public bool IsCollection => false;

        public bool IsEntity => false;

        public bool IsFragment => false;

        public bool IsComplexType => true;

        public bool IsValue => false;

        public Type ItemType => null;

        public IEnumerable<string> MemberNames => new string[0];

        public Type Type => typeof(Dictionary<string, object>);

        public bool UsePatchProxy => false;

        public Expression Construct(Expression context)
        {
            return New(typeof(Dictionary<string, object>));
        }

        public IMemberOptions GetMember(string memberName)
        {
            return new DictionaryMemberOptions(memberName);
        }
    }
}
