using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Events
{
    public class EntityUpdatingEventArgs : EventArgs
    {
        public NavigationEntry ParentNavigationEntry { get; set; }

        public EntityEntry ParentEntityEntry
        {
            get
            {
                return ParentNavigationEntry?.EntityEntry;
            }
        }

        public object Entity { get; set; }
    }
}
