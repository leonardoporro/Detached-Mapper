using AgileObjects.ReadableExpressions.Extensions;

namespace Detached.Mappers.Context
{
    public class MapperContextEntry
    {
        public object Entity { get; set; }

        public MapperActionType ActionType { get; set; }

        public override string ToString()
        {
            return $"{Entity?.GetType().GetFriendlyName()} [{ActionType}] (Entry)";
        }
    }
}