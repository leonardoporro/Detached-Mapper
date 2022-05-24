using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.TypeOptions.Types.Dictionary
{
    public class DictionaryTypeOptions : ITypeOptions
    {
        public Dictionary<string, object> Annotations { get; set; }

        public bool IsCollectionType => false;

        public bool IsEntityType => false;

        public bool IsFragment => false;

        public bool IsComplexType => true;

        public bool IsPrimitiveType => false;

        public Type ItemType => null;

        public IEnumerable<string> MemberNames => new string[0];

        public Type Type => typeof(Dictionary<string, object>);

        public bool UsePatchProxy => false;

        public string DiscriminatorName => null;

        public Dictionary<object, Type> DiscriminatorValues => null;

        public Expression Construct(Expression context, Expression discriminator)
        {
            return New(typeof(Dictionary<string, object>));
        }

        public IMemberOptions GetMember(string memberName)
        {
            return new DictionaryMemberOptions(memberName);
        }
    }
}
