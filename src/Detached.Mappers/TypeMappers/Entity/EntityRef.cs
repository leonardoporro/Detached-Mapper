using System;

namespace Detached.Mappers.TypeMappers.Entity
{
    public class EntityRef
    {
        public EntityRef(IEntityKey key, Type clrType)
        {
            Key = key;
            ClrType = clrType;
        }
        public IEntityKey Key { get; }

        public Type ClrType { get; }

        public override bool Equals(object obj)
        {
            EntityRef other = obj as EntityRef;
            return other != null && other.Key == Key && other.ClrType == ClrType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, ClrType);
        }
    }
}
