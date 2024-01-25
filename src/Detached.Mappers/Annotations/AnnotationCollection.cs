using System.Collections.Generic;

namespace Detached.Mappers.Annotations
{
    public class AnnotationCollection : Dictionary<string, object>
    {
        public Annotation<TValue> Annotation<TValue>(string name)
        {
            return new Annotation<TValue>(this, name);
        }
    }
}