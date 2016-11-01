using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins
{
    public interface IDetachedPlugin : IDisposable
    {
        void Initialize(IDetachedContext detachedContext);

        bool IsEnabled { get; set; }

        int Priority { get; }
    }
}
