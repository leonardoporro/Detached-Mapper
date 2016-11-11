using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    public class KeyServicesCache : IKeyServicesCache
    {
        private readonly ConcurrentDictionary<Tuple<Type, Type>, IKeyServices> _cache
            = new ConcurrentDictionary<Tuple<Type, Type>, IKeyServices>();

        public IKeyServices GetOrCreate(Type dbContextType, Type entityType, Func<IKeyServices> factory)
        {
            Tuple<Type, Type> cacheKey = new Tuple<Type, Type>(dbContextType, entityType);
            IKeyServices keyServices;
            if (!_cache.TryGetValue(cacheKey, out keyServices))
            {
                keyServices = factory();
                _cache[cacheKey] = keyServices;
            }
            return keyServices;
        }
    }
}
