using System;
using System.Diagnostics.CodeAnalysis;

namespace Detached.Mappers.EntityFramework
{
    public struct EntityMapperKey
    {
        public EntityMapperKey(Type dbContextType, object profileKey)
        {
            ProfileKey = profileKey;
            DbContextType = dbContextType;
        }

        public Type DbContextType { get; }

        public object ProfileKey { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(EntityMapperKey), DbContextType, ProfileKey);
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            return obj is EntityMapperKey other && Equals(other.ProfileKey, ProfileKey) && Equals(other.DbContextType, DbContextType);
        }
    }
}