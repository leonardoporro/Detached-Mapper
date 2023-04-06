using System;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal class InheritanceDiscriminatorAttribute : Attribute
    {
        public string Value { get; set; }
        public Type EntityType { get; set; }
        public Type MappingType { get; set; }
    }
}