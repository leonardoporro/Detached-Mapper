using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Detached.Services
{
    public interface ILoadServices
    {
        IQueryable<TEntity> GetBaseQuery<TEntity>() where TEntity : class;

        void GetIncludePaths(IEntityType entityType, HashSet<IEntityType> visited, string currentPath, ref List<string> paths);

        Task<TEntity> LoadAsync<TEntity>(params object[] keyValues) where TEntity : class;

        Task<List<TEntity>> LoadAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig) where TEntity : class;

        Task<List<TResult>> LoadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TEntity : class
            where TResult : class;

        Task<TEntity> LoadPersisted<TEntity>(TEntity entity) where TEntity : class;

        Task<TEntity> LoadPersisted<TEntity>(object[] keyValues) where TEntity : class;
    }
}