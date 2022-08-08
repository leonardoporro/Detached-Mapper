using Detached.Mappers.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers.POCO.Inherited
{
    public class InheritedTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public InheritedTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return ((sourceType.IsComplex() || sourceType.IsEntity()) && !sourceType.IsAbstract())
                 && (targetType.IsComplex() || targetType.IsEntity())
                 && targetType.IsInherited();
        }

        public ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            string targetMemberName = targetType.GetDiscriminatorName();
            string sourceMemberName = _options.GetSourcePropertyName(sourceType, targetType, targetMemberName);

            IMemberOptions discriminatorMember = sourceType.GetMember(sourceMemberName);
            if (discriminatorMember == null)
            {
                throw new MapperException($"Discriminator member {targetType.GetDiscriminatorName()} does not exist in type {targetType.ClrType}");
            }

            var getDiscriminator =
                    Lambda(
                        typeof(Func<,,>).MakeGenericType(sourceType.ClrType, typeof(IMapContext), discriminatorMember.ClrType),
                        Parameter(typePair.SourceType, out Expression sourceExpr),
                        Parameter(typeof(IMapContext), out Expression contextExpr),
                        discriminatorMember.BuildGetExpression(sourceExpr, contextExpr)
                    ).Compile();

            Type tableType = typeof(Dictionary<,>).MakeGenericType(discriminatorMember.ClrType, typeof(ILazyTypeMapper));
            IDictionary table = (IDictionary)Activator.CreateInstance(tableType);

            foreach (var entry in targetType.GetDiscriminatorValues())
            {
                ILazyTypeMapper mapper = _options.GetLazyTypeMapper(new TypePair(sourceType.ClrType, entry.Value, typePair.Flags));
                table.Add(entry.Key, mapper);
            }

            Type mapperType = typeof(InheritedTypeMapper<,,>).MakeGenericType(sourceType.ClrType, targetType.ClrType, discriminatorMember.ClrType);
            return (ITypeMapper)Activator.CreateInstance(mapperType, new object[] { getDiscriminator, table });
        }
    }
}
