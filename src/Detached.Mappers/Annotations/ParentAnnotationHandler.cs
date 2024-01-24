using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class ParentAnnotationHandler : AnnotationHandler<ParentAttribute>
    {
        public override void Apply(ParentAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Parent(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class ParentAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_PARENT_REFERENCE";

        public static bool IsParent(this ITypeMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static bool IsParent(this TypePairMember member)
        {
            return member.Annotations.ContainsKey(VALUE_KEY);
        }

        public static void Parent(this ITypeMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Parent<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Parent(value);
            return member;
        }

        public static void Parent(this TypePairMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value; 
        }

        public static TypePairMemberBuilder<TType, TMember> Parent<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Parent();
            return member;
        }

        public static bool IsParentConfigured(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(VALUE_KEY);
        }
    }
}