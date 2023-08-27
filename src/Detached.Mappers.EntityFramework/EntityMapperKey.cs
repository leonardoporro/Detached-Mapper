using System;

namespace Detached.Mappers.EntityFramework
{
    public class EntityMapperKey
    {
        public EntityMapperKey(Type dbContextType, object profileKey)
        {
            DbContextType = dbContextType;
            ProfileKey = profileKey;
        }

        public Type DbContextType { get; }

        public object ProfileKey { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(EntityMapperKey), DbContextType, ProfileKey);
        }

        public override bool Equals(object obj)
        {
            return obj is EntityMapperKey other
                && DbContextType == other.DbContextType
                && Equals(ProfileKey, other.ProfileKey);
        }
    }
}
