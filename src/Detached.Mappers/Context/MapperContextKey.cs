using AgileObjects.ReadableExpressions.Extensions;
using System;

namespace Detached.Mappers.Context
{
    public class MapperContextKey
    {
        public MapperContextKey(Type entityType, object keyValue)
        {
            EntityType = entityType;
            KeyValue = keyValue;
        }

        public object KeyValue { get; }

        public Type EntityType { get; }

        public override bool Equals(object obj)
        {
            MapperContextKey other = obj as MapperContextKey;

            return other != null
                && Equals(other.EntityType, EntityType)
                && Equals(other.KeyValue, KeyValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(MapperContextKey), EntityType, KeyValue);
        }

        public override string ToString()
        {
            return $"{KeyValue} [{EntityType.GetFriendlyName()}] (Key)";
        }
    }
}