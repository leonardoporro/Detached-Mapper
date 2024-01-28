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
                var keyMember = targetType.GetKeyMember();

                foreach (string targetMemberName in memberNames)
                {
                    ITypeMember targetMember = targetType.GetMember(targetMemberName);

                    if (targetMember != null)
                    {
                        TypePairMember member = new TypePairMember();

                        member.TargetType = targetType;
                        member.SourceType = sourceType;
                        member.TargetMember = targetMember;

                        string sourceMemberName = GetSourcePropertyName(mapperOptions, sourceType, targetType, targetMemberName);

                        ITypeMember sourceMember = sourceType.GetMember(sourceMemberName);

                        if (sourceMember == null)
                        {
                            string keyName = targetMemberName + keyMember.Name;
                            sourceMember = sourceType.GetMember(keyName);
                        }

                        member.SourceMember = sourceMember;

                        typePair.Members.Add(targetMemberName, member);
                    }
                }
            }

            return typePair;
        }

        public string GetSourcePropertyName(MapperOptions mapperOptions, IType sourceType, IType targetType, string memberName)
        {
            for (int i = mapperOptions.PropertyNameConventions.Count - 1; i >= 0; i--)
            {
                memberName = mapperOptions.PropertyNameConventions[i].GetSourcePropertyName(sourceType, targetType, memberName);
            }

            return memberName;
        }
    }
}
