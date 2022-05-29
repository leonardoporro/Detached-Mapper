using AgileObjects.ReadableExpressions.Extensions;
using System;

namespace Detached.Mappers.Tests.Mocks
{
    public class MapContextKey
    {
        public MapContextKey(Type entityType, object keyValue)
        {
            EntityType = entityType;
            KeyValue = keyValue;
        }

        public object KeyValue { get; }

        public Type EntityType { get; }

        public override bool Equals(object obj)
        {
            MapContextKey other = obj as MapContextKey;

            return other != null
                && Equals(other.EntityType, EntityType)
                && Equals(other.KeyValue, KeyValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(MapContextKey), EntityType, KeyValue);
        }

        public override string ToString()
        {
            return $"{KeyValue} [{EntityType.GetFriendlyName()}] (Key)";
        }
    }
}