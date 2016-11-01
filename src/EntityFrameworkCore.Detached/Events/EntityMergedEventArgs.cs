using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Events
{
    public class EntityMergedEventArgs : EntityUpdatedEventArgs
    {
        public object DetachedEntity { get; set; }

        public bool Modified { get; set; }
    }
}
