using System;
using System.Linq.Expressions;

namespace Detached.Expressions
{
    public class VariableDefinitionExpression : ParameterDefinitionExpression
    {
        public VariableDefinitionExpression(string name, Expression initialize, out Expression param)
            : base(name, initialize.Type, out param)
        {
            InitializeExpression = initialize;
        }

        public VariableDefinitionExpression(string name, Type type, out Expression param)
            : base(name, type, out param)
        {

        }

        public VariableDefinitionExpression(ParameterExpression variable, Expression initialize)
            : base(variable)
        {
            InitializeExpression = initialize;
        }

        public VariableDefinitionExpression(ParameterExpression parameter)
            : base(parameter)
        {
        }

        public Expression InitializeExpression { get; }
    }
}