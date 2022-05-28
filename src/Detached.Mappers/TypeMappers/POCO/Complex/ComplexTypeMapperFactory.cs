using Detached.Mappers.TypeOptions;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeMappers.POCO.Complex
{
    public class ComplexTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public ComplexTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsComplex
                && targetType.IsComplex
                && !targetType.IsEntity;
        }

        public ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            ExpressionBuilder builder = new ExpressionBuilder(_options);

            LambdaExpression construct = builder.BuildNewExpression(targetType);

            LambdaExpression mapMembers = builder.BuildMapMembersExpression(typePair, sourceType, targetType, (s, t) => true);
                
            Type mapperType = typeof(ComplexTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);

            return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { construct.Compile(), mapMembers.Compile() });
        }
    }
}