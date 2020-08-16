using Detached.Model;

namespace Detached.Mapping
{
    public class BackReferenceMap
    {
        public TypeMap Parent { get; set; }

        public IMemberOptions MemberOptions { get; set; }

        public ITypeOptions MemberTypeOptions { get; set; }
    }
}