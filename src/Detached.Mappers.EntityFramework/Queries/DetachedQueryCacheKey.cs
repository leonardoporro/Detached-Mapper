using System;

namespace Detached.Mappers.EntityFramework.Queries
{
    public enum QueryType { Projection, Load }

    public class DetachedQueryCacheKey
    {
        public DetachedQueryCacheKey(Type sourceType, Type targetType, QueryType queryType)
        {
            SourceType = sourceType;
            TargetType = targetType;
            QueryType = queryType;
        }

        public Type SourceType { get; }

        public Type TargetType { get; }

        public QueryType QueryType { get; }

        public override bool Equals(object obj)
        {
            var other = obj as DetachedQueryCacheKey;
            return other != null
                && SourceType == other.SourceType
                && TargetType == other.TargetType
                && QueryType == other.QueryType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(DetachedQueryCacheKey), SourceType, TargetType, QueryType);
        }
    }
}
