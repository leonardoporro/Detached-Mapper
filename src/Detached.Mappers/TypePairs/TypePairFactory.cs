using Detached.Mappers.Types;
using System.Collections.Generic;

namespace Detached.Mappers.TypePairs
{
    public class TypePairFactory : ITypePairFactory
    {
        public TypePair Create(MapperOptions mapperOptions, IType sourceType, IType targetType, TypePairMember parentMember)
        {
            TypePair typePair = new TypePair(sourceType, targetType, parentMember);

            IEnumerable<string> memberNames = targetType.MemberNames;

            if (memberNames != null)
            {
                foreach (string targetMemberName in memberNames)
                {
                    ITypeMember targetMember = targetType.GetMember(targetMemberName);

                    if (targetMember != null)
                    {
                        TypePairMember member = new TypePairMember();

                        member.TargetType = targetType;
                        member.SourceType = sourceType;
                        member.TargetMember = targetMember;
                        member.SourceMember = GetSourceMember(targetMemberName, sourceType, targetType, mapperOptions);

                        typePair.Members.Add(targetMemberName, member);
                    }
                }
            }

            return typePair;
        }

        public ITypeMember GetSourceMember(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            ITypeMember result = null;

            for (int i = mapperOptions.MemberNameConventions.Count - 1; i >= 0; i--)
            {
                var convention = mapperOptions.MemberNameConventions[i];

                var sourceMemberName = convention.GetSourceMemberName(targetMemberName, sourceType, targetType, mapperOptions);

                if (sourceMemberName != null)
                {
                    result = sourceType.GetMember(sourceMemberName);

                    if (result != null)
                        break;
                }
            }

            return result;
        }
    }
}