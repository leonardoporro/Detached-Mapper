using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Events
{
    public class EntityUpdatedEventArgs : EventArgs
    {
        public NavigationEntry ParentNavigationEntry { get; set; }

        public EntityEntry ParentEntityEntry
        {
            get

            {
                return ParentNavigationEntry?.EntityEntry;
            }
        }

        public EntityEntry EntityEntry { get; set; }
    }
}
