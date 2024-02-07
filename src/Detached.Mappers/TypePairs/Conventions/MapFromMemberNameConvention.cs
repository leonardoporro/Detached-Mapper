
using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs.Conventions
{
    internal class MapFromMemberNameConvention : IMemberNameConvention
    {
        public string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            string sourceMemberName = null;

            var member = targetType.GetMember(targetMemberName);
            var annotation = member.Annotations.MapFrom();

            if (annotation != null && annotation.IsDefined())
            {
                annotation.Value().TryGetValue(sourceType.ClrType, out sourceMemberName);
            }

            return sourceMemberName;
        }
    }
}
