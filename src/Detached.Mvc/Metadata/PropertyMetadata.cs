using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Metadata
{
    public class PropertyMetadata : TypeMetadata
    {
        #region Ctor.

        public PropertyMetadata()
        {
        }

        public PropertyMetadata(IDictionary<string, string> values)
            : base(values)
        {
        }

        #endregion

        public string Property
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
            }
        }

        public string MetaProperty
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
            }
        }
    }
}
