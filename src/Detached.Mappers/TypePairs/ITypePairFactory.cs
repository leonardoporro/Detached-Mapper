using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs
{
    public interface ITypePairFactory
    {
        TypePair Create(MapperOptions mapperOptions, IType sourceType, IType targetType, TypePairMember sourceMember);
    }
}