using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EntityFrameworkCore.Detached.Plugins
{
    public abstract class DetachedPlugin : IDetachedPlugin
    {
        public DetachedPlugin()
        {
            Name = GetType().Name;
        }

        public virtual int Priority { get; set; } = 0;

        public virtual string Name { get; set; }

        public virtual bool IsEnabled { get; set; } = true;
    }
}