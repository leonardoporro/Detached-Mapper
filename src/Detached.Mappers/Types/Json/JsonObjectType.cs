using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.Types.Json
{
    public class JsonObjectType : IType
    {
        public Type ClrType => typeof(JsonObject);

        public Type ItemClrType => null;

        public AnnotationCollection Annotations { get; } = new();

        public MappingSchema MappingSchema => MappingSchema.Complex;

        public IEnumerable<string> MemberNames => new string[0];

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            return New(ClrType);
        }

        public ITypeMember GetMember(string memberName)
        {
            return new JsonObjectMember(memberName);
        }
    }
}
