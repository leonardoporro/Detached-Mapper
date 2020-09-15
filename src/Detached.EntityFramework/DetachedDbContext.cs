using Detached.EntityFramework.Context;
using Detached.EntityFramework.Conventions;
using Detached.EntityFramework.Queries;
using Detached.Mapping;
using Detached.Mapping.Context;
using Detached.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework
{
    public class DetachedDbContext : DbContext
    {
        public DetachedDbContext(DbContextOptions options)
            : base(options)
        {
            ModelOptions modelOptions = new ModelOptions();
            modelOptions.Conventions.Add(new IsEntityConvention(this));
            OnMapperCreating(modelOptions);
            Mapper = new Mapper(Options.Create(modelOptions), new TypeMapFactory());
            QueryProvider = new QueryProvider(Mapper);
        }

        public DetachedDbContext(DbContextOptions options, Mapper mapper, QueryProvider queryProvider)
            : base(options)
        {
            Mapper = mapper;
            QueryProvider = queryProvider;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TEntity : class
            where TProjection : class
        {
            return QueryProvider.Project<TProjection, TEntity>(query);
        }

        protected virtual Mapper Mapper { get; }

        protected virtual QueryProvider QueryProvider { get; }

        protected virtual void OnMapperCreating(ModelOptions options)
        {
        }

        public Task<TEntity> MapAsync<TEntity>(object entityOrDTO, MapperOptions mapperOptions = null)
            where TEntity : class
        {
            return Task.Run(() => Map<TEntity>(entityOrDTO, mapperOptions));
        }

        public TEntity Map<TEntity>(object entityOrDTO, MapperOptions mapperOptions = null)
            where TEntity : class
        {
            if (mapperOptions == null)
                mapperOptions = new MapperOptions();

            var context = new EntityFrameworkMapperContext(this, QueryProvider, mapperOptions);

            return (TEntity)Mapper.Map(entityOrDTO, entityOrDTO.GetType(), null, typeof(TEntity), context);
        }
    }
}