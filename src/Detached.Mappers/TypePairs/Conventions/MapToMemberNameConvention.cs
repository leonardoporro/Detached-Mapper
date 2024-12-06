
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs.Conventions
{
    internal class MapToMemberNameConvention : IMemberNameConvention
    {
        public string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            string sourceMemberName = null;

            if (sourceType.MemberNames != null)
            {
                foreach (var memberName in sourceType.MemberNames)
                {
                    var member = sourceType.GetMember(memberName);

                    var mapToAnnotation = member.Annotations.MapTo();

                    if (mapToAnnotation != null
                        && mapToAnnotation.IsDefined()
                        && mapToAnnotation.Value().TryGetValue(targetType.ClrType, out var mapToMemberName)
                        && mapToMemberName == targetMemberName)
                    {
                        sourceMemberName = memberName;
                        break;
                    }
                }
            }

            return sourceMemberName;
        }
    }
}
