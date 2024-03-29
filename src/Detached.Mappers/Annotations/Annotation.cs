﻿namespace Detached.Mappers.Annotations
{
    public class Annotation<TValue>
    {
        public Annotation(AnnotationCollection collection, string name)
        {
            Collection = collection;
            Name = name;
        }

        public string Name { get; }

        public AnnotationCollection Collection { get; }

        public bool IsDefined()
        {
            return Collection.ContainsKey(Name);
        }

        public void Reset()
        {
            Collection.Remove(Name);
        }

        public void Set(TValue value)
        {
            Collection[Name] = value;
        }

        public TValue Value()
        {
            TValue result = default;

            if (Collection.TryGetValue(Name, out object value))
            {
                return (TValue)value;
            }

            return result;
        }
    }
}
