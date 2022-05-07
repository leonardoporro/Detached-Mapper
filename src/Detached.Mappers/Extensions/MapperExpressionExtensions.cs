using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Detached.Mappers.Exceptions;
using static System.Linq.Expressions.Expression;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using System.Text.Json;

namespace Detached.Mappers.Extensions
{
    public static class MapperExpressionExtensions
    {
        static string JsonSerialize(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static Expression JsonSerialize(Expression expression)
        {
            return Call("JsonSerialize", typeof(MapperExpressionExtensions), expression);
        }

        public static Expression ThrowMapperException(string message, params Expression[] args)
        {
            Expression messageExpr;

            if (args != null && args.Length > 0)
            {
                IReadOnlyList<Expression> paramList = args.Select(p => Convert(p, typeof(object))).ToList();

                List<Expression> parameters = new List<Expression>();
                parameters.Add(Constant(message));
                parameters.AddRange(paramList);

                messageExpr = Call("Format", typeof(string), parameters.ToArray());
 
            }
            else
            {
                messageExpr = Constant(message);
            }

            return Throw(New(typeof(MapperException), new[] { messageExpr }));
        }
    }
}
