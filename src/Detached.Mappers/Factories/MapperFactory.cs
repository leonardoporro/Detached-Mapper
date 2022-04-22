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

namespace Detached.Mappers.Factories
{
    public abstract class MapperFactory
    {
        public abstract bool CanMap(TypeMap typeMap);

        public abstract LambdaExpression Create(TypeMap typeMap);

        public Type GetDelegateType(TypeMap typeMap)
        {
            return typeof(MapperDelegate<,>).MakeGenericType(
                typeMap.Source.Type,
                typeMap.Target.Type);
        }

        protected virtual Expression CallMapper(TypeMap typeMap, Expression source, Expression target)
        {
            if (typeMap.MapReference != null)
                return Invoke(typeMap.MapReference, source, target, typeMap.Context);
            else if (!typeMap.Target.Type.IsAssignableFrom(typeMap.Source.Type))
                throw new MapperException($"Type {typeMap.Source.Type.GetFriendlyName()} is not assignable to {typeMap.Source.Type.GetFriendlyName()}. Did you miss a CreateMap call?");
            else
                return source;
        }

        protected virtual Expression CreateMapper(TypeMap typeMap)
        {
            if (typeMap.Mapper.ShouldMap(typeMap.SourceOptions, typeMap.TargetOptions))
            {
                if (typeMap.MapReference == null)
                {
                    string mapperName = $"{typeMap.Source}_{typeMap.Target}_mapper";

                    typeMap.MapReference = Parameter(GetDelegateType(typeMap), mapperName);

                    Expression mapperExpr = typeMap.Mapper.GetFactory(typeMap).Create(typeMap);

                    return Variable(typeMap.MapReference, mapperExpr);
                }
            }

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
            return Call("OnMapperAction", typeMap.Context, typeMap.Target, typeMap.Source, typeMap.TargetKey, Constant(actionType));
        }
    }
}