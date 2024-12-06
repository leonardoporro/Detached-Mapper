using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs.Conventions
{
    public class CamelCaseMemberNameConvention : IMemberNameConvention
    {
        public string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            return char.ToLower(targetMemberName[0]) + targetMemberName.Substring(1);
        } 
    }
}
