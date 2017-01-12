using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Events
{
    public class EntityMergingEventArgs : EntityUpdatingEventArgs
    {
        public object DetachedEntity { get; set; }
    }
}
