using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IDetachedServices : IDisposable
    {
        IDetachedContext DetachedContext { get; }

        IEventManager EventManager { get; }

        IPluginManager PluginManager { get; }

        IUpdateServices UpdateServices { get; }

        ILoadServices LoadServices { get; }

        void Initialize(IDetachedContext detachedContext, IServiceProvider scopedProvider);
    }
}
