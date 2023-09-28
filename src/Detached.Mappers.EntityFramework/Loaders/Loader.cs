using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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
            ILoaderQuery query = GetQuery(targetType, entityOrDto);

            return query.Load(dbContext, entityOrDto);
        }

        public Task<object> LoadAsync(DbContext dbContext, Type entityType, object entityOrDto)
        {
            ILoaderQuery query = GetQuery(entityType, entityOrDto);

            return query.LoadAsync(dbContext, entityOrDto);
        }

        ILoaderQuery GetQuery(Type targetType, object entityOrDto)
        {
            return _queries.GetOrAdd(new LoaderQueryKey(entityOrDto.GetType(), targetType), 
                        key => _queryFactory.Create(key.SourceType, key.TargetType));
        } 
    }
}