using System;

namespace Detached.Mappers.Cache
{
    public class MapperCacheKey
    {
        public MapperCacheKey(Type sourceType, Type targetType, bool generic)
        {
            SourceType = sourceType;
            TargetType = targetType;
            Generic = generic;
        }

        public Type SourceType { get; }

        public Type TargetType { get; }

        public bool Generic { get; }

        public override bool Equals(object obj)
        {
            var other = obj as MapperCacheKey;
            return other != null
                && SourceType == other.SourceType
                && TargetType == other.TargetType
                && Generic == other.Generic;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(MapperCacheKey), SourceType, TargetType, Generic);
        }
    }
}