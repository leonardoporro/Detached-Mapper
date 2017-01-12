using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Events
{
    public class EntityLoadedEventArgs : EventArgs
    {
        public INavigation ParentNavigation { get; set; }

        public IEntityType EntityType { get; set; }

        public object Entity { get; set; }
    }
}
