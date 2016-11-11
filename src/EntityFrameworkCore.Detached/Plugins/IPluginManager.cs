using System;

namespace EntityFrameworkCore.Detached.Plugins
{
    public interface IPluginManager
    {
        IDetachedPlugin this[Type type] { get; }

        IDetachedPlugin this[string name] { get; }

        void EnableAllPlugins();

        void DisableAllPlugins();

        void Dispose();
    }
}