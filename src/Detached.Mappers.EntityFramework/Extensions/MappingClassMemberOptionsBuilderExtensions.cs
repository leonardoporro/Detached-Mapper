using Detached.Mappers.Annotations;
using Detached.Mappers.TypeOptions.Class.Builder;

namespace Detached.Mappers.EntityFramework
{
    public static class MappingClassMemberOptionsBuilderExtensions
    {
        public static ClassMemberOptionsBuilder<TType, TMember> Association<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member)
        {
            member.MemberOptions.IsAssociation();
            return member;
        }

        public static ClassMemberOptionsBuilder<TType, TMember> Composition<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member)
        {
            member.MemberOptions.IsComposition();
            return member;
        }
    }
}