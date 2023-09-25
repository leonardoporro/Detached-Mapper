using Detached.Mappers.TypeMappers.Entity;
using Detached.Mappers.Types;

namespace Detached.Mappers
{
    public enum MapAction { Load, Create, Update, Delete, Attach }

    public class StackEntry
    {
        public StackEntry(IEntityKey key, IType sourceType, object source, IType targetType, object target, MapAction action)
        {
            Key = key;
            SourceType = sourceType;
            Source = source;
            TargetType = targetType;
            Target = target;
        }
        public IEntityKey Key { get; }

        public IType SourceType { get; }

        public object Source { get; }

        public IType TargetType { get; }

        public object Target { get; }

        public MapAction MapAction { get; } 
    }
}