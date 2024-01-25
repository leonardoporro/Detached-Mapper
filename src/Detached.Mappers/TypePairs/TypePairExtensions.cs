namespace Detached.Mappers.TypePairs
{
    public static class TypePairExtensions
    {
        public static bool IsMapped(this TypePairMember member)
        {
            return !member.IsIgnored()
                && member.TargetMember != null
                && member.TargetMember.CanWrite
                && !member.TargetMember.IsIgnored()
                && ((member.SourceMember != null
                    && member.SourceMember.CanRead
                    && !member.SourceMember.IsIgnored())
                        || member.TargetMember.IsParent());
        }
    }
}
