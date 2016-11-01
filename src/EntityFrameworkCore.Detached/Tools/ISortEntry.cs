using System.Linq;

namespace EntityFrameworkCore.Detached.Tools
{
    public interface ISortEntry<TEntity>
    {
        IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);

        IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
    }
}
