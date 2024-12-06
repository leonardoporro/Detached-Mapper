using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Context;
using Detached.Mappers.Options;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using System.Linq.Expressions;
using System.Reflection;

namespace Detached.Mappers.Types.Class.Builder
{
    public class ClassTypeBuilder<TType>
    {
        public ClassTypeBuilder(ClassType typeOptions, MapperOptions mapperOptions)
        {
            ClassType = typeOptions;
            Options = mapperOptions;
        }

        public ClassType ClassType { get; }

        public MapperOptions Options { get; }

        public virtual ClassTypeBuilder<TNewType> Type<TNewType>()
        {
            var newType = (ClassType)Options.GetType(typeof(TNewType));

            return new ClassTypeBuilder<TNewType>(newType, Options);
        }

        public ClassTypeMemberBuilder<TType, TMember> Member<TMember>(Expression<Func<TType, TMember>> selector)
        {
            ClassTypeMember memberOptions = GetMember(selector);

            return new ClassTypeMemberBuilder<TType, TMember>(ClassType, memberOptions, Options);
        }

        public TypePairBuilder<TSource, TType> FromType<TSource>()
        {
            IType sourceType = Options.GetType(typeof(TSource));
            IType targetType = Options.GetType(typeof(TType));

            TypePair typePair = Options.GetTypePair(sourceType, targetType, null);

            return new TypePairBuilder<TSource, TType>(Options, typePair);
        }

        public ClassTypeBuilder<TType> Constructor(Expression<Func<IMapContext, TType>> constructor)
        {
            ClassType.Constructor = constructor;
            return this;
        }

        public ClassTypeBuilder<TType> IncludeAll()
        {
            foreach (var memberName in ClassType.MemberNames)
            {
                ClassType.GetMember(memberName).Ignore(false);
            }

            return this;
        }

        public ClassTypeBuilder<TType> ExcludeAll()
        {
            foreach (var memberName in ClassType.MemberNames)
            {
                ClassType.GetMember(memberName).Ignore(true);
            }

            return this;
        }

        public ClassTypeBuilder<TType> IncludePrimitives()
        {
            foreach (var memberName in ClassType.MemberNames)
            {
                ITypeMember memberOptions = ClassType.GetMember(memberName);
                if (Options.IsPrimitive(memberOptions.ClrType))
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
            ClassType.Annotations.DiscriminatorName().Set(memberOptions.Name);

            return new ClassTypeDiscriminatorBuilder<TType, TMember>(ClassType, Options);
        }

        public ClassTypeBuilder<TType> Key(params Expression<Func<TType, object>>[] members)
        {
            foreach (string memberName in ClassType.MemberNames)
            {
                ITypeMember memberOptions = ClassType.GetMember(memberName);
                memberOptions.Key(false);
            }

            foreach (LambdaExpression selector in members)
            {
                var convert = selector.Body as UnaryExpression;
                var member = convert.Operand as MemberExpression;
                var propInfo = member.Member as PropertyInfo;

                if (ClassType.Members.TryGetValue(propInfo.Name, out ClassTypeMember memberOptions))
                {
                    memberOptions.Key(true);
                }
            }

            return this;
        }

        ClassTypeMember GetMember<TMember>(Expression<Func<TType, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            if (!ClassType.Members.TryGetValue(memberName, out ClassTypeMember memberOptions))
                throw new ArgumentException($"Member {memberName} does not exist.");

            return memberOptions;
        }
    }
}