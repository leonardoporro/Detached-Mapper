using AgileObjects.ReadableExpressions.Extensions;
using Detached.Mappers.Context;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMaps;
using Detached.RuntimeTypes.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories
{
    public abstract class MapperFactory
    {
        public abstract bool CanMap(TypeMap typeMap);

        public abstract LambdaExpression Create(TypeMap typeMap);

        public Type GetDelegateType(TypeMap typeMap)
        {
            return typeof(MapperDelegate<,>).MakeGenericType(
                typeMap.SourceExpression.Type,
                typeMap.TargetExpression.Type);
        }

        protected virtual Expression CallMapper(TypeMap typeMap, Expression source, Expression target)
        {
            if (typeMap.MappingFunctionExpression != null)
                return Invoke(typeMap.MappingFunctionExpression, source, target, typeMap.BuildContextExpression);
            else if (!typeMap.TargetExpression.Type.IsAssignableFrom(typeMap.SourceExpression.Type))
                throw new MapperException($"Type {typeMap.SourceExpression.Type.GetFriendlyName()} is not assignable to {typeMap.SourceExpression.Type.GetFriendlyName()}. Did you miss a CreateMap call?");
            else
                return source;
        }

        protected virtual Expression CreateMapper(TypeMap typeMap)
        {
            //if (typeMap.Mapper.ShouldMap(typeMap.SourceTypeOptions, typeMap.TargetTypeOptions))
            //{
            //    if (typeMap.MappingFunctionExpression == null)
            //    {
            //        string mapperName = $"{typeMap.SourceExpression}_{typeMap.TargetExpression}_mapper";

            //        typeMap.MappingFunctionExpression = Parameter(GetDelegateType(typeMap), mapperName);

            //        Expression mapperExpr = typeMap.Mapper.GetFactory(typeMap).Create(typeMap);

            //        return Variable(typeMap.MappingFunctionExpression, mapperExpr);
            //    }
            //}

            return Empty();
        }

        protected virtual IncludeExpression CreateMemberMappers(TypeMap typeMap, Func<MemberMap, bool> filter = null)
        {
            List<Expression> memberMappers = new List<Expression>();

            foreach (MemberMap memberMap in typeMap.Members)
            {
                if (filter == null || filter(memberMap))
                {
                    memberMappers.Add(CreateMapper(memberMap.TypeMap));
                }
            }

            return Include(memberMappers);
        }

        protected virtual Expression OnMapperAction(TypeMap typeMap, MapperActionType actionType)
        {
            return Call("OnMapperAction", typeMap.BuildContextExpression, typeMap.TargetExpression, typeMap.SourceExpression, typeMap.TargetKeyExpression, Constant(actionType));
        }
    }
}