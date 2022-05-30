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

        public TypeKind Kind => TypeKind.Complex;

        public Dictionary<string, object> Annotations { get; set; }
 
        public bool IsAbstract => false;

        public bool IsInherited => false;
 
        public IEnumerable<string> MemberNames => new string[0];
 
        public string DiscriminatorName => null;

        public Dictionary<object, Type> DiscriminatorValues => null;
 
        public Expression BuildIsSetExpression(Expression instance, Expression context, string memberName)
        {
            return Call("ContainsKey", instance, Constant(memberName));
        }

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