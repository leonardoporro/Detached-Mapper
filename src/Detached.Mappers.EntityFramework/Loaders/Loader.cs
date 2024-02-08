using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Loaders
{
    public class Loader
    {
        readonly ConcurrentDictionary<LoaderQueryKey, ILoaderQuery> _queries
            = new ConcurrentDictionary<LoaderQueryKey, ILoaderQuery>();

        readonly MapperOptions _options;
        readonly LoaderQueryFactory _queryFactory;


        public Loader(MapperOptions options)
        {
            _options = options;
            _queryFactory = new LoaderQueryFactory(options);
        }

        public object Load(DbContext dbContext, Type targetType, object entityOrDto)
        {
            if (entityOrDto == null)
                return Task.FromResult<object>(null);

            ILoaderQuery query = GetQuery(entityOrDto.GetType(), targetType);

            return query.Load(dbContext, entityOrDto);
        }

        public IEnumerable<object> Load(DbContext dbContext, Type targetType, IEnumerable<object> entityOrDto)
        {
            if (entityOrDto == null)
                return Array.Empty<object>();

            ILoaderQuery query = GetQuery(entityOrDto.GetType(), targetType);

            return query.Load(dbContext, entityOrDto);
        }

        public Task<object> LoadAsync(DbContext dbContext, Type entityType, object entityOrDto)
        {
            if (entityOrDto == null)
                return Task.FromResult<object>(null);

            ILoaderQuery query = GetQuery(entityOrDto.GetType(), entityType);

            return query.LoadAsync(dbContext, entityOrDto);
        }

        public Task<IEnumerable<object>> LoadAsync(DbContext dbContext, Type entityType, IEnumerable<object> entitiesOrDtos)
        {
            var item = entitiesOrDtos.FirstOrDefault();
            if (item == null)
            {
                return Task.FromResult<IEnumerable<object>>(Array.Empty<object>());
            }
            else
            {
                Type sourceType = item.GetType();

                ILoaderQuery query = GetQuery(sourceType, entityType);

                return query.LoadAsync(dbContext, entitiesOrDtos);
            }
        }

        ILoaderQuery GetQuery(Type sourceType, Type targetType)
        {
            var key = new LoaderQueryKey(sourceType, targetType);

            return _queries.GetOrAdd(key, key => _queryFactory.Create(key.SourceType, key.TargetType));
        }
    }
}