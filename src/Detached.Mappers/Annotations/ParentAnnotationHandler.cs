using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class ParentAnnotationHandler : AnnotationHandler<ParentAttribute>
    {
        public override void Apply(ParentAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Parent(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class ParentAnnotationHandlerExtensions
    {
        public const string KEY = "DETACHED_PARENT_REFERENCE";

        public static bool IsParent(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Parent(this ITypeMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static ClassTypeMemberBuilder<TType, TMember> Parent<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Parent(value);
            return member;
        }

        public static bool IsParent(this TypePairMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Parent(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> Parent<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Exclude();
            return member;
        }
    }
}