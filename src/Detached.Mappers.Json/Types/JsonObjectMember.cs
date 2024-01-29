using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonObjectMember : ITypeMember
    {
        readonly string _memberName;

        public JsonObjectMember(string memberName)
        {
            _memberName = memberName;
        }

        public AnnotationCollection Annotations { get; } = new();

        public string Name => _memberName;

        public Type ClrType => typeof(JsonNode);

        public bool CanRead => true;

        public bool CanWrite => true;

        public bool CanTryGet => true;

        public Expression BuildGetExpression(Expression instance, Expression context)
        {
            return Index(instance, Constant(_memberName));
        }

        public Expression BuildSetExpression(Expression instance, Expression value, Expression context)
        {
            return Assign(Index(instance, Constant(_memberName)), value);
        }

        public Expression BuildTryGetExpression(Expression instance, Expression context, Expression outVar)
        {
            return Call("TryGetPropertyValue", instance, Constant(Name), outVar);
        }
    }
}
