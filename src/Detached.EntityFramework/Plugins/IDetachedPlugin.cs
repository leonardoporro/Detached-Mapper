using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Plugins
{
    /// <summary>
    /// Detached plugin contract.
    /// </summary>
    public interface IDetachedPlugin : IDisposable
    {
        /// <summary>
        /// Gets or sets wheather the plugin is enabled or not.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets the plugin priority that affects the load order among the other plugins.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executed fist time the plugin is loaded, when all the other plugins and 
        /// services are also loaded.
        /// </summary>
        void Initialize();
    }
}
