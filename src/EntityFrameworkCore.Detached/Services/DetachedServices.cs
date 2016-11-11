using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Detached.Services
{
    public class DetachedServices : IDetachedServices
    {
        #region Fields

        IDetachedContext _detachedContext;
        IEventManager _eventManager;
        IPluginManager _pluginManager;
        ILoadServices _loadServices;
        IUpdateServices _updateServices;
        IServiceProvider _scopedProvider;

        #endregion

        #region Ctor.

        public DetachedServices()
        {
        }

        #endregion

        #region Properties

        public IDetachedContext DetachedContext
        {
            get
            {
                return _detachedContext;
            }
        }

        public IEventManager EventManager
        {
            get
            {
                return _eventManager ?? (_eventManager = _scopedProvider.GetService<IEventManager>());
            }
        }

        public IPluginManager PluginManager
        {
            get
            {
                return _pluginManager ?? (_pluginManager = _scopedProvider.GetService<IPluginManager>());
            }
        }

        public IUpdateServices UpdateServices
        {
            get
            {
                return _updateServices ?? (_updateServices = _scopedProvider.GetService<IUpdateServices>());
            }
        }

        public ILoadServices LoadServices
        {
            get
            {
                return _loadServices ?? (_loadServices = _scopedProvider.GetService<ILoadServices>());
            }
        }

        #endregion

        public void Initialize(IDetachedContext detachedContext,
                               IServiceProvider scopedProvider)
        {
            _detachedContext = detachedContext;
            _scopedProvider = scopedProvider;
        }

        public void Dispose()
        {
            if (_pluginManager != null)
            {
                _pluginManager.Dispose();
                _pluginManager = null;
            }
        }
    }
}
