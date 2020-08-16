using System;
using System.Collections.Generic;
using System.Linq;

namespace Detached.Reflection
{
    public class MethodSignature
    {
        public MethodSignature(Type type, string name, IReadOnlyList<Type> paramTypes, IReadOnlyList<Type> genericArgTypes)
        {
            Type = type;
            Name = name;
            ParameterTypes = paramTypes;
            GenericArguments = genericArgTypes;
        }

        public Type Type { get; }

        public string Name { get; }

        public IReadOnlyList<Type> ParameterTypes { get; }

        public IReadOnlyList<Type> GenericArguments { get; }

        public override bool Equals(object obj)
        {
            MethodSignature other = obj as MethodSignature;
            return other != null
                && Equals(other.Name, Name)
                && Equals(other.Type, Type)
                && TypesEqual(other.ParameterTypes, ParameterTypes)
                && TypesEqual(other.GenericArguments, ParameterTypes);
        }

        bool TypesEqual(IEnumerable<Type> first, IEnumerable<Type> second)
        {
            if (first == null || second == null)
                return true;
            else if (first == null)
                return false;
            else if (second == null)
                return false;
            else
                return Enumerable.SequenceEqual(first, second);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();

            hashCode.Add(Name);
            hashCode.Add(Type);

            if (ParameterTypes != null)
            {
                foreach (Type type in ParameterTypes)
                    hashCode.Add(type);
            }

            if (GenericArguments != null)
            {
                foreach (Type arg in GenericArguments)
                    hashCode.Add(arg);
            }

            return hashCode.ToHashCode();
        }
    }
}