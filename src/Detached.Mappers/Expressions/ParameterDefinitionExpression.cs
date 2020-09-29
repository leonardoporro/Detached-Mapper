using System;
using System.Linq.Expressions;

namespace Detached.Mappers.Expressions
{
    public class ParameterDefinitionExpression : ExtendedExpression
    {
        public ParameterDefinitionExpression(ParameterExpression parameter)
        {
            ParameterExpression = parameter;
        }

        public ParameterDefinitionExpression(string name, Type type, out Expression param)
        {
            ParameterExpression = Parameter(type, name);
            param = ParameterExpression;
        }

        public ParameterExpression ParameterExpression { get; protected set; }

        public override Type Type => ParameterExpression.Type;

        public override Expression Reduce()
        {
            return ParameterExpression;
        }

        public override string ToString()
        {
            return $".{ParameterExpression.Type} {ParameterExpression.Name}";
        }
    }
}