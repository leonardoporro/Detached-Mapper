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
    public class ManyToManyEntity<T1, T2> : IManyToManyEntity
    {
        /// <summary>
        /// Gets a 1 to * navigation property pointing to <see cref="T1"/>.
        /// </summary>
        public T1 End1 { get; set; }

        /// <summary>
        /// Gets a 1 to * navigation property pointing to <see cref="T2"/>.
        /// </summary>
        public T2 End2 { get; set; }

        object IManyToManyEntity.End1
        {
            get
            {
                return End1;
            }
            set
            {
                End1 = (T1)value;
            }
        }

        object IManyToManyEntity.End2
        {
            get
            {
                return End2;
            }

            set
            {
                End2 = (T2)value;
            }
        }
    }

    public interface IManyToManyEntity
    {
        object End1 { get; set; }

        object End2 { get; set; }
    }
}
