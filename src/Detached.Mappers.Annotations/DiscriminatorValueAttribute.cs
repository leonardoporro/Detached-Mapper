using System;

namespace Detached.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DiscriminatorValueAttribute : Attribute
    {
        public DiscriminatorValueAttribute(object value, Type concreteType)
        {
            Value = value;
            ConcreteType = concreteType;
        }

        public object Value { get; }

        public Type ConcreteType { get; set; }
    }
}