using System;

namespace Detached.EntityFramework.Plugins
{
    /// <summary>
    /// Handles all the plugin instances.
    /// </summary>
    public interface IPluginManager : IDisposable
    {
        /// <summary>
        /// Gets a plugin by its name.
        /// </summary>
        /// <param name="name">The plugin name.</param>
        IDetachedPlugin this[string name] { get; }

        /// <summary>
        /// Gets a plugin instance by its type.
        /// </summary>
        /// <typeparam name="TPlugin">Clr type of the plugin to get.</typeparam>
        TPlugin Get<TPlugin>() where TPlugin : class;

        /// <summary>
        /// Initializes all plugins.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Enables all plugins.
        /// </summary>
        void Enable();

        /// <summary>
        /// Disables all plugins.
        /// </summary>
        void Disable();

        /// <summary>
        /// Releases all internal resources.
        /// </summary>
        void Dispose();
    }
}