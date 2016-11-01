using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Events
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public PropertyEntry Property { get; set; }

        public object CurrentValue
        {
            get
            {
                return Property?.CurrentValue;
            }
        }

        public object OldValue { get; set; }
    }
}
