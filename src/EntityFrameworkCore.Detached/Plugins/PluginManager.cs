using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins
{
    public class PluginManager : IPluginManager
    {
        List<IDetachedPlugin> _plugins;

        public PluginManager(IServiceProvider _serviceProvider)
        {
            _plugins = _serviceProvider.GetServices<IDetachedPlugin>()
                                       .OrderByDescending(p => p.Priority)
                                       .ToList();
        }

        public void Initialize()
        {
            foreach (IDetachedPlugin plugin in _plugins)
                plugin.Initialize();
        }

        public void EnableAll()
        {
            foreach (IDetachedPlugin plugin in _plugins)
                plugin.IsEnabled = true;
        }

        public void DisableAll()
        {
            foreach (IDetachedPlugin plugin in _plugins)
                plugin.IsEnabled = false;
        }

        public IDetachedPlugin this[string name]
        {
            get
            {
                foreach (IDetachedPlugin plugin in _plugins)
                {
                    if (string.Equals(plugin.Name, name, StringComparison.CurrentCultureIgnoreCase))
                        return plugin; 
                }
                return null;
            }
        }

        public IDetachedPlugin this[Type type]
        {
            get
            {
                foreach (IDetachedPlugin plugin in _plugins)
                {
                    if (plugin.GetType() == type)
                        return plugin;
                }
                return null;
            }
        }

        public void Dispose()
        {
            foreach (IDetachedPlugin plugin in _plugins)
            {
                plugin.Dispose();
            }
        }
    }
}
