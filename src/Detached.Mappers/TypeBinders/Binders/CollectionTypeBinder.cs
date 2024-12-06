using Detached.Mappers.Options;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeBinders.Binders
{
    public class CollectionTypeBinder : ITypeBinder
    {
        public bool CanBind(Mapper mapper, TypePair typePair)
        {
            return typePair.SourceType.IsCollection() && typePair.TargetType.IsCollection();
        }

        public Expression Bind(Mapper mapper, TypePair typePair, Expression sourceExpr)
        {
            MapperOptions options = mapper.Options;

            IType entityItemType = options.GetType(typePair.SourceType.ItemClrType);
            IType projectionItemType = options.GetType(typePair.TargetType.ItemClrType);
            TypePair itemTypePair = options.GetTypePair(entityItemType, projectionItemType, typePair.ParentMember);

            var param = Parameter(entityItemType.ClrType, "e");
            var itemBinder = mapper.GetTypeBinder(itemTypePair);
            var itemMap = itemBinder.Bind(mapper, itemTypePair, param);

            if (itemMap != null)
            {
                LambdaExpression bodyExpr = ToLambda(entityItemType.ClrType, param, itemMap);
                return Call("ToList", typeof(Enumerable), Call("Select", typeof(Enumerable), sourceExpr, bodyExpr));
            }
            else
            {
                return null;
            }
        }

        LambdaExpression ToLambda(Type type, ParameterExpression paramExpr, Expression body)
        {
            Type funcType = typeof(Func<,>).MakeGenericType(type, body.Type);
            var lambda = Expression.Lambda(funcType, body, paramExpr);
            return lambda;
        }
    }
}