using AgileObjects.ReadableExpressions.Extensions;
using Detached.Model;

namespace Detached.Mapping
{
    public class MemberMap
    {
        public IMemberOptions SourceOptions { get; set; }

        public IMemberOptions TargetOptions { get; set; }

        public TypeMap TypeMap { get; set; }

        public bool IsComposition => TargetOptions.IsComposition;

        public bool IsKey => TargetOptions.IsKey;

        public bool IsBackReference { get; set; }

        public override string ToString() 
            => $"{SourceOptions.Name} [{SourceOptions.Type.GetFriendlyName()}] to {TargetOptions.Name} [{TargetOptions.Type.GetFriendlyName()}] (MemberMap)";
    }
}