using Detached.Mappers.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Types.Dictionary
{
    public class DictionaryType : IType
    {
        public Type ClrType => typeof(Dictionary<string, object>);

        public Type ItemClrType => null;

        public MappingSchema MappingSchema => MappingSchema.Complex;

        public Dictionary<string, object> Annotations { get; set; }

        public IEnumerable<string> MemberNames => new string[0];

        public bool IsAbstract { get; set; } = false;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            return New(typeof(Dictionary<string, object>));
        }

        public ITypeMember GetMember(string memberName)
        {
            return new DictionaryTypeMember(memberName);
        }
    }
}