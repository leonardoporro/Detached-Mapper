using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Provides extra methods to EntityType.
    /// </summary>
    public static class EntityTypeExtensions
    {
        /// <summary>
        /// Gets a serialized primary key to be used as key in a hash table.
        /// </summary>
        /// <param name="entityType">The entity metadata.</param>
        /// <param name="instance">The instance whose key is to be retrieved.</param>
        /// <returns>A string containing a serialized primary key value.</returns>
        public static string GetKeyForHashTable(this EntityType entityType, object instance)
        {
            StringBuilder builder = new StringBuilder();
            Key key = entityType.FindPrimaryKey();

            foreach (Property prop in key.Properties)
            {
                builder.Append(prop.Name);
                builder.Append("=");
                builder.Append(prop.Getter.GetClrValue(instance));
                builder.Append("&");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates a hash table from the given enumerable to do lookups in O(1).
        /// </summary>
        /// <param name="entityType">Metadata of the enitity.</param>
        /// <param name="list">List of entities to convert to a hash table.</param>
        /// <returns>A hash table in the form [PrimaryKey] -> [Entity].</returns>
        public static Dictionary<string, object> CreateHashTable(this EntityType entityType, IEnumerable list)
        {
            Dictionary<string, object> table = new Dictionary<string, object>();
            if (list != null)
            {
                foreach (object item in list)
                {
                    if (item != null)
                    {
                        table.Add(GetKeyForHashTable(entityType, item), item);
                    }
                }
            }
            return table;
        }

        public static object[] GetKeyValues (this Key key, object instance)
        {
            return key.Properties.Select(p => p.Getter.GetClrValue(instance)).ToArray();
        }

        public static object[] GetKeyValues(this EntityType entityType, object instance)
        {
            Key key = entityType.FindPrimaryKey();
            return key.Properties.Select(p => p.Getter.GetClrValue(instance)).ToArray();
        }
    }
}
