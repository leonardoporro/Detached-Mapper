using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Detached.Mappers.Types.Class.Builder
{
    public class ClassTypeBuilder<TType>
    {
        public ClassTypeBuilder(ClassType typeOptions, MapperOptions mapperOptions)
        {
            TypeOptions = typeOptions;
            MapperOptions = mapperOptions;
        }

        public ClassType TypeOptions { get; }

        public MapperOptions MapperOptions { get; }

        public ClassTypeMemberBuilder<TType, TMember> Member<TMember>(Expression<Func<TType, TMember>> selector)
        {
            ClassTypeMember memberOptions = GetMember(selector);

            return new ClassTypeMemberBuilder<TType, TMember>(TypeOptions, memberOptions, MapperOptions);
        }

        public TypePairBuilder<TSource, TType> FromType<TSource>()
        {
            IType sourceType = MapperOptions.GetType(typeof(TSource));
            IType targetType = MapperOptions.GetType(typeof(TType));

            TypePair typePair = MapperOptions.GetTypePair(sourceType, targetType, null);

            return new TypePairBuilder<TSource, TType>(MapperOptions, typePair);
        }

        public ClassTypeBuilder<TType> Constructor(Expression<Func<IMapContext, TType>> constructor)
        {
            TypeOptions.Constructor = constructor;
            return this;
        }

        public ClassTypeBuilder<TType> IncludeAll()
        {
            foreach (var memberName in TypeOptions.MemberNames)
            {
                TypeOptions.GetMember(memberName).IsNotMapped(false);
            }

            return this;
        }

        public ClassTypeBuilder<TType> ExcludeAll()
        {
            foreach (var memberName in TypeOptions.MemberNames)
            {
                TypeOptions.GetMember(memberName).IsNotMapped(false);
            }

            return this;
        }

        public ClassTypeBuilder<TType> IncludePrimitives()
        {
            foreach (var memberName in TypeOptions.MemberNames)
            {
                ITypeMember memberOptions = TypeOptions.GetMember(memberName);
                if (MapperOptions.IsPrimitive(memberOptions.ClrType))
                {
                    memberOptions.IsNotMapped(false);
                }
                else
                {
                    memberOptions.IsNotMapped(true);
                }
            }

            return this;
        }

        public ClassTypeDiscriminatorBuilder<TType, TMember> Discriminator<TMember>(Expression<Func<TType, TMember>> selector)
        {
            ClassTypeMember memberOptions = GetMember(selector);
            TypeOptions.SetDiscriminatorName(memberOptions.Name);

            return new ClassTypeDiscriminatorBuilder<TType, TMember>(TypeOptions);
        }

        public ClassTypeBuilder<TType> Key(params Expression<Func<TType, object>>[] members)
        {
            foreach (string memberName in TypeOptions.MemberNames)
            {
                ITypeMember memberOptions = TypeOptions.GetMember(memberName);
                memberOptions.IsKey(false);
            }

            foreach (LambdaExpression selector in members)
            {
                var convert = selector.Body as UnaryExpression;
                var member = convert.Operand as MemberExpression;
                var propInfo = member.Member as PropertyInfo;

                if (TypeOptions.Members.TryGetValue(propInfo.Name, out ClassTypeMember memberOptions))
                {
                    memberOptions.IsKey(true);
                }
            }

            return this;
        }

        ClassTypeMember GetMember<TMember>(Expression<Func<TType, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            if (!TypeOptions.Members.TryGetValue(memberName, out ClassTypeMember memberOptions))
                throw new ArgumentException($"Member {memberName} does not exist.");

            return memberOptions;
        }
    }
}