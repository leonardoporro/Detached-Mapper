using Detached.EntityFramework.Events;
using Detached.EntityFramework.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.EntityFramework.Services
{
    public class DetachedContextServices : DbContextServices, IDetachedContextServices, IDbContextServices
    {
        #region Fields

        IDetachedContext _detachedContext;
        IEventManager _eventManager;
        IPluginManager _pluginManager;
        ILoadServices _loadServices;
        IUpdateServices _updateServices;
        IServiceProvider _scopedProvider;
        DetachedOptionsExtension _options;

        #endregion

        #region Ctor.

        public DetachedContextServices()
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

        public DetachedOptionsExtension DetachedOptions
        {
            get
            {
                return _options;
            }
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return _scopedProvider;
            }
        }

        #endregion

        public override IDbContextServices Initialize(IServiceProvider scopedProvider, IDbContextOptions contextOptions, DbContext context)
        {
            _scopedProvider = scopedProvider;
            _options = contextOptions.FindExtension<DetachedOptionsExtension>();
            return base.Initialize(scopedProvider, contextOptions, context);
        }

        public void SetDetachedContext(IDetachedContext detachedContext)
        {
            _detachedContext = detachedContext;
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
