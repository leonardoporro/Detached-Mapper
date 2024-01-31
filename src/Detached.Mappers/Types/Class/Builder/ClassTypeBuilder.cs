using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Detached.Mappers.Types.Class.Builder
{
    public class ClassTypeBuilder<TType>
    {
        public ClassTypeBuilder(ClassType typeOptions, MapperOptions mapperOptions)
        {
            Type = typeOptions;
            MapperOptions = mapperOptions;
        }

        public ClassType Type { get; }

        public MapperOptions MapperOptions { get; }

        public ClassTypeMemberBuilder<TType, TMember> Member<TMember>(Expression<Func<TType, TMember>> selector)
        {
            ClassTypeMember memberOptions = GetMember(selector);

            return new ClassTypeMemberBuilder<TType, TMember>(Type, memberOptions, MapperOptions);
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
            Type.Constructor = constructor;
            return this;
        }

        public ClassTypeBuilder<TType> IncludeAll()
        {
            foreach (var memberName in Type.MemberNames)
            {
                Type.GetMember(memberName).Ignore(false);
            }

            return this;
        }

        public ClassTypeBuilder<TType> ExcludeAll()
        {
            foreach (var memberName in Type.MemberNames)
            {
                Type.GetMember(memberName).Ignore(true);
            }

            return this;
        }

        public ClassTypeBuilder<TType> IncludePrimitives()
        {
            foreach (var memberName in Type.MemberNames)
            {
                ITypeMember memberOptions = Type.GetMember(memberName);
                if (MapperOptions.IsPrimitive(memberOptions.ClrType))
                {
                    memberOptions.Ignore(false);
                }
                else
                {
                    memberOptions.Ignore(true);
                }
            }

            return this;
        }

        public ClassTypeDiscriminatorBuilder<TType, TMember> Discriminator<TMember>(
            Expression<Func<TType, TMember>> selector)
        {
            ClassTypeMember memberOptions = GetMember(selector);
            Type.Annotations.DiscriminatorName().Set(memberOptions.Name);

            return new ClassTypeDiscriminatorBuilder<TType, TMember>(Type);
        }

        public ClassTypeBuilder<TType> Key(params Expression<Func<TType, object>>[] members)
        {
            foreach (string memberName in Type.MemberNames)
            {
                ITypeMember memberOptions = Type.GetMember(memberName);
                memberOptions.Key(false);
            }

            foreach (LambdaExpression selector in members)
            {
                var convert = selector.Body as UnaryExpression;
                var member = convert.Operand as MemberExpression;
                var propInfo = member.Member as PropertyInfo;

                if (Type.Members.TryGetValue(propInfo.Name, out ClassTypeMember memberOptions))
                {
                    memberOptions.Key(true);
                }
            }

            return this;
        }

        ClassTypeMember GetMember<TMember>(Expression<Func<TType, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            if (!Type.Members.TryGetValue(memberName, out ClassTypeMember memberOptions))
                throw new ArgumentException($"Member {memberName} does not exist.");

            return memberOptions;
        }
    }
}