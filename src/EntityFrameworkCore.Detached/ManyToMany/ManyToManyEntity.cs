using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.ManyToMany
{
    /// <summary>
    /// Provides a ClrType for many to many intermediate tables.
    /// </summary>
    /// <typeparam name="T1">Entity type for the first table.</typeparam>
    /// <typeparam name="T2">Entity type for the second table.</typeparam>
    public class ManyToManyEntity<T1, T2>
    {
        /// <summary>
        /// Gets a 1 to * navigation property pointing to <see cref="T1"/>.
        /// </summary>
        public T1 End1 { get; set; }

        /// <summary>
        /// Gets a 1 to * navigation property pointing to <see cref="T2"/>.
        /// </summary>
        public T2 End2 { get; set; }
    }
}
