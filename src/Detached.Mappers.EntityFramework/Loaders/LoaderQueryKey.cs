using System;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public struct LoaderQueryKey
    {
        public LoaderQueryKey(Type sourceType, Type targetType)
        {
            SourceType = sourceType;
            TargetType = targetType; 
        }

        public Type SourceType { get; }

        public Type TargetType { get; }  

        public override bool Equals(object obj)
        {
            return obj is LoaderQueryKey other
                && SourceType == other.SourceType
                && TargetType == other.TargetType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(LoaderQueryKey), SourceType, TargetType);
        }
    }
}
