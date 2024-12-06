using Detached.Mappers.Context;
using Detached.Mappers.Types;

namespace Detached.Mappers.Tests.Mocks
{
    public class MapContextEntry
    {
        public object Entity { get; set; }

        public MapperActionType ActionType { get; set; }

        public override string ToString()
        {
            return $"{Entity?.GetType().GetFriendlyName()} [{ActionType}] (Entry)";
        }
    }
}