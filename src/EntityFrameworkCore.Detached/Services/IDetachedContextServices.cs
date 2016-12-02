using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    /// <summary>
    /// Provides services for the detached context.
    /// </summary>
    public interface IDetachedContextServices : IDisposable
    {
        /// <summary>
        /// Gets the scoped service provider.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the detached context.
        /// </summary>
        IDetachedContext DetachedContext { get; }

        /// <summary>
        /// Gets the event manager. Use it for subscribing events such as 'EntityAdded'.
        /// </summary>
        IEventManager EventManager { get; }

        /// <summary>
        /// Gets the plugin manager. Use it for enable/disable plugins.
        /// </summary>
        IPluginManager PluginManager { get; }

        /// <summary>
        /// Gets the update services that handle Detached Entity Graph persistence.
        /// </summary>
        IUpdateServices UpdateServices { get; }

        /// <summary>
        /// Gets the load services that handle the Detached Entity Graph loading.
        /// </summary>
        ILoadServices LoadServices { get; }

        /// <summary>
        /// Gets the detached options.
        /// </summary>
        DetachedOptionsExtension DetachedOptions { get; }

        /// <summary>
        /// Sets the current detached context. 
        /// </summary>
        /// <param name="detachedContext">Owning detached context.</param>
        void SetDetachedContext(IDetachedContext detachedContext);
    }
}
