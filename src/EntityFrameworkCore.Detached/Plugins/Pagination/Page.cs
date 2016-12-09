using System.Collections.Generic;

namespace EntityFrameworkCore.Detached.Plugins.Pagination
{
    public class Page<TEntity> : IPage<TEntity>
    {
        public List<TEntity> Items { get; set; }

        public int PageCount { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }

        public int RowCount { get; set; }
    }
}
