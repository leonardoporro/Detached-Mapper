using System;
using System.Diagnostics.CodeAnalysis;

namespace Detached.Mappers.EntityFramework.Profiles
{
    public struct ProfileKey
    {
        public static ProfileKey Empty { get; } = new ProfileKey(null);

        public ProfileKey(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(ProfileKey), Value);
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            return obj is ProfileKey other && Equals(other.Value, Value);
        }
    }
}