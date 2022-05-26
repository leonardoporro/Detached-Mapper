using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.TypeOptions.Dictionary
{
    public class DictionaryMemberOptions : IMemberOptions
    {
        readonly string _name;

        public DictionaryMemberOptions(string name)
        {
            _name = name;
        }

        public Dictionary<string, object> Annotations { get; set; } = new Dictionary<string, object>();

        public bool Ignored => false;

        public bool IsKey => false;

        public string Name => _name;

        public bool IsComposition => false;

        public Type ClrType => typeof(object);

        public bool CanRead => true;

        public bool CanWrite => true;

        public Expression BuildGetterExpression(Expression instance, Expression context)
        {
            return Index(instance, Constant(_name));
        }

        public Expression BuildSetterExpression(Expression instance, Expression value, Expression context)
        {
            return Assign(Index(instance, Constant(_name)), value);
        }
    }
}