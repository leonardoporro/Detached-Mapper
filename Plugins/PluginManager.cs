using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins
{
    public class PluginManager
    {
        DbContext _dbContext;
        List<IDetachedPlugin> _plugins = new List<IDetachedPlugin>();

        public PluginManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void OnRootLoaded<TEntity>(TEntity root)
        {
            
        }
    }
}
