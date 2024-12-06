using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs.Conventions
{
    public class DefaultMemberNameConvention : IMemberNameConvention
    {
        public string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            return targetMemberName;
        }
    }
}
