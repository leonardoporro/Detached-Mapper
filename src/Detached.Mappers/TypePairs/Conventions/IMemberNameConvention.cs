using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs.Conventions
{
    public interface IMemberNameConvention
    {
        string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions);
    }
}
