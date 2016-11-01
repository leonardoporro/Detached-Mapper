using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.ManyToMany
{
    public class ManyToManyPlugin : IDetachedPlugin
    {
        IDetachedContext _detachedContext;

        public bool IsEnabled { get; set; } = true;

        public int Priority { get; set; } = 0;

        public void Initialize(IDetachedContext detachedContext)
        {
            _detachedContext = detachedContext;
        }

        public void Dispose()
        {
        } 
    }
}
