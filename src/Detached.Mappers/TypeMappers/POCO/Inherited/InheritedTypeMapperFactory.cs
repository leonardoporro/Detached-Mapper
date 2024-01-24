using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMappers.POCO.Abstract;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
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
        public bool CanCreate(Mapper mapper, TypePair typePair)
        {
            return ((typePair.SourceType.IsComplex() || typePair.SourceType.IsEntity()) && !typePair.SourceType.IsAbstract())
               && (typePair.TargetType.IsComplex() || typePair.TargetType.IsEntity())
               && typePair.TargetType.IsInherited();
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair)
        {
            string targetMemberName = typePair.TargetType.GetDiscriminatorName();

            if (!typePair.Members.TryGetValue(targetMemberName, out TypePairMember member) || member.IsIgnored() || member.SourceMember == null)
            {
                throw new MapperException($"Discriminator member {targetMemberName} does not exist in type {typePair.SourceType.ClrType}");
            }

            string sourceMemberName = member.SourceMember.Name;

            ITypeMember discriminatorMember = typePair.SourceType.GetMember(sourceMemberName);
            if (discriminatorMember == null)
            {
                throw new MapperException($"Discriminator member {typePair.TargetType.GetDiscriminatorName()} does not exist in type {typePair.TargetType.ClrType}");
            }

            var getDiscriminator =
                    Lambda(
                        typeof(Func<,,>).MakeGenericType(typePair.SourceType.ClrType, typeof(IMapContext), discriminatorMember.ClrType),
                        Parameter(typePair.SourceType.ClrType, out Expression sourceExpr),
                        Parameter(typeof(IMapContext), out Expression contextExpr),
                        discriminatorMember.BuildGetExpression(sourceExpr, contextExpr)
                    ).Compile();

            Type tableType = typeof(Dictionary<,>).MakeGenericType(discriminatorMember.ClrType, typeof(ITypeMapper));
            IDictionary table = (IDictionary)Activator.CreateInstance(tableType);

            foreach (var entry in typePair.TargetType.GetDiscriminatorValues())
            {
                IType sourceDiscriminatorType = typePair.SourceType;
                IType targetDiscriminatorType = mapper.Options.GetType(entry.Value);
                TypePair discriminatorTypePair = mapper.Options.GetTypePair(sourceDiscriminatorType, targetDiscriminatorType, typePair.ParentMember);


                var discrimiatorMapperType = typeof(AbstractTypeMapper<,>)
                    .MakeGenericType(sourceDiscriminatorType.ClrType, targetDiscriminatorType.ClrType);

                var discriminatorMapper = (ITypeMapper)Activator.CreateInstance(discrimiatorMapperType, mapper, discriminatorTypePair, targetDiscriminatorType.ClrType);

                table.Add(entry.Key, discriminatorMapper);
            }

            Type mapperType = typeof(InheritedTypeMapper<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, discriminatorMember.ClrType);
            return (ITypeMapper)Activator.CreateInstance(mapperType, getDiscriminator, table);
        }
    }
}