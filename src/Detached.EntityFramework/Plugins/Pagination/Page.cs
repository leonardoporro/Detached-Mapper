using System.Collections.Generic;

namespace Detached.EntityFramework.Plugins.Pagination
{
    public class Page<TEntity> : IPage<TEntity>
    {
        public List<TEntity> Items { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }
    }
}
