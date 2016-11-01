using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Events
{
    public class PropertyChangingEventArgs : EventArgs
    {
        public PropertyEntry Property { get; set; }

        public object CurrentValue
        {
            get
            {
                return Property?.CurrentValue;
            }
        }

        public object NewValue { get; set; }

        public bool Cancel { get; set; }
    }
}
