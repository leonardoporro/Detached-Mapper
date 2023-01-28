using Detached.Mappers.Types;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Detached.Mappers.TypePairs
{
    public class TypePairMember
    {
        public IType SourceType { get; set; }

        public IType TargetType { get; set; }

        public ITypeMember SourceMember { get; set; }

        public ITypeMember TargetMember { get; set; }

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();
    }
}