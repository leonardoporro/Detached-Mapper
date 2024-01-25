using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class ParentAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_PARENT_REFERENCE";

        public static bool IsParent(this ITypeMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Parent(this ITypeMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Parent<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.Member.Parent(value);
            return member;
        }

        public static void Parent(this TypePairMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static bool IsParentConfigured(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(VALUE_KEY);
        }
    }
}