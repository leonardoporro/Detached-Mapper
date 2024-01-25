namespace Detached.Mappers.Annotations
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

        public bool HasValue()
        {
            if (Collection.ContainsKey(Name))
            {
                return true;
            }
            else if (Collection.Sources != null)
            {
                for (int i = Collection.Count - 1; i >= 0; i--)
                {
                    if (Collection.Annotation<TValue>(Name).HasValue())
                    {
                        return true;
                    }
                }
            }

            return false;
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
            else if (Collection.Sources != null)
            {
                for (int i = Collection.Count - 1; i >= 0; i--)
                {
                    var annotation = Collection.Annotation<TValue>(Name);

                    if (annotation.IsDefined())
                    {
                        result = annotation.Value();
                    }
                }
            }

            return result;
        }
    }
}
