using Detached.Mappers.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Types.Dictionary
{
    public class DictionaryTypeMember : ITypeMember
    {
        readonly string _name;

        public DictionaryTypeMember(string name)
        {
            _name = name;
        }

        public AnnotationCollection Annotations { get; set; } = new();

        public bool IsIgnored => false;

        public bool IsKey => false;

        public string Name => _name;

        public Type ClrType => typeof(object);

        public bool CanRead => true;

        public bool CanWrite => true;

        public bool CanTryGet => true;

        public Expression BuildGetExpression(Expression instance, Expression context)
        {
            return Index(instance, Constant(_name));
        }

        public Expression BuildSetExpression(Expression instance, Expression value, Expression context)
        {
            return Assign(Index(instance, Constant(_name)), value);
        }

        public Expression BuildTryGetExpression(Expression instance, Expression context, Expression outVar)
        {
            return Call("TryGetValue", instance, Constant(Name), outVar);
        }
    }
}