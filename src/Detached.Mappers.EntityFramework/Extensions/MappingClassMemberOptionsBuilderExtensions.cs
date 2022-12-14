using Detached.Mappers.Annotations;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.EntityFramework
{
    public static class MappingClassMemberOptionsBuilderExtensions
    {
        public static ClassTypeMemberBuilder<TType, TMember> Association<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member)
        {
            member.MemberOptions.IsAssociation();
            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Composition<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member)
        {
            member.MemberOptions.IsComposition();
            return member;
        }
    }
}