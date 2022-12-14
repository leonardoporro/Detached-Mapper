using AgileObjects.ReadableExpressions;
using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.POCO.Complex
{
    public class ComplexTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(MapperOptions mapperOptions, TypePair typePair)
        {
            return typePair.SourceType.IsComplex()
                         && typePair.TargetType.IsComplex()
                         && !typePair.TargetType.IsEntity();
        }

        public ITypeMapper Create(MapperOptions mapperOptions, TypePair typePair)
        {
            ExpressionBuilder builder = new ExpressionBuilder(mapperOptions);

            LambdaExpression construct = builder.BuildNewExpression(typePair.TargetType);

            LambdaExpression mapMembers = builder.BuildMapMembersExpression(typePair, (s, t) => true);

            string code = mapMembers.ToReadableString();

            Type mapperType = typeof(ComplexTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);

            return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { construct.Compile(), mapMembers.Compile() });
        }
    }
}