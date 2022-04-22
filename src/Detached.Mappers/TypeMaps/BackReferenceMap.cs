using Detached.Mappers.TypeOptions;

namespace Detached.Mappers.TypeMaps
{
    public class BackReferenceMap
    {
        public TypeMap Parent { get; set; }

        public IMemberOptions MemberOptions { get; set; }

        public ITypeOptions MemberTypeOptions { get; set; }
    }
}