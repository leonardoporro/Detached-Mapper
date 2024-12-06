using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.Types.Json
{
    public class JsonArrayType : IType
    {
        public Type ClrType => typeof(JsonArray);

        public Type ItemClrType => typeof(JsonNode);

        public AnnotationCollection Annotations { get; } = new();

        public MappingSchema MappingSchema => MappingSchema.Collection;

        public IEnumerable<string> MemberNames => null;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            return New(ClrType);
        }

        public ITypeMember GetMember(string memberName)
        {
            throw new NotImplementedException();
        }
    }
}
