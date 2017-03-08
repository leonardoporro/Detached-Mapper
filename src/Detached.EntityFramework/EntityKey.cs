using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework
{
    public class KeyValue
    {
        public KeyValue(object keyValue)
        {
            Values = new object[] { keyValue };
        }

        public KeyValue(IEnumerable<object> values)
        {
            this.Values = values.ToArray();
        }

        public object[] Values { get; }

        public override bool Equals(object obj)
        {
            bool result = true;
            KeyValue key = obj as KeyValue;
            if (key == null)
                result = false;
            else if (this.Values.Length != key.Values.Length)
                result = false;
            else
            {
                for(int i = 0; i < Values.Length; i++)
                {
                    if (!Equals(Values[i], key.Values[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (object value in Values)
            {
                unchecked
                {
                    hash = hash * 31 + value.GetHashCode();
                }
            }
            return hash;
        }
    }
}
