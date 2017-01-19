using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Detached.Mvc.Metadata
{
    public class TypeMetadata
    {
        #region Fields

        IDictionary<string, string> _values;

        #endregion

        #region Ctor.

        public TypeMetadata()
        {
            _values = new Dictionary<string, string>();
        }

        public TypeMetadata(IDictionary<string,string> values)
        {
            _values = new Dictionary<string, string>(values);
        }

        #endregion

        #region Properties

        public string this[string property]
        {
            get
            {
                return GetValue(property);
            }
            set
            {
                _values[property] = value;
            }
        }

        public string Application
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

        public string Module
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

        public string Feature
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

        public string Layer
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

        public string Tier
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

        #endregion

        public string GetValue([CallerMemberName]string property = null)
        {
            string value;
            _values.TryGetValue(property.ToLower(), out value);
            return value;
        }

        public void SetValue(string value, [CallerMemberName]string property = null)
        {
            _values[property.ToLower()] = value;
        }
    }
}