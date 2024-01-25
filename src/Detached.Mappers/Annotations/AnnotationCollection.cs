using System.Collections.Generic;

namespace Detached.Mappers.Annotations
{
    public class AnnotationCollection : Dictionary<string, object>
    {
        public AnnotationCollection(ICollection<AnnotationCollection> sources = null)
        {
            Sources = sources;
        }

        public ICollection<AnnotationCollection> Sources { get; }

        public Annotation<TValue> Annotation<TValue>(string name)
        {
            return new Annotation<TValue>(this, name);
        }
    }
}