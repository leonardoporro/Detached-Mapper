using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Plugins.ManyToMany
{
    public class ManyToManyMetadata
    {
        public ManyToManyTableMetadata TableMetadata { get; set; }

        public ManyToManyCollectionMetadata CollectionMetadata { get; set; }

        public void ToCollection(object entity)
        {
            IEnumerable tableData = TableMetadata.Getter.GetClrValue(entity) as IEnumerable;
            if (tableData != null)
            {
                IList collectionData = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(CollectionMetadata.ItemType));
                foreach (object tableItem in tableData)
                {
                    object value = TableMetadata.End2Getter.GetClrValue(tableItem);
                    collectionData.Add(value);
                }
                CollectionMetadata.Setter.SetClrValue(entity, collectionData);
            }
        }

        public void ToTable(object entity)
        {
            IEnumerable collectionData = CollectionMetadata.Getter.GetClrValue(entity) as IEnumerable;
            if (collectionData != null)
            {
                IList tableData = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(TableMetadata.ItemType));
                foreach (object collectionItem in collectionData)
                {
                    object tableItem = Activator.CreateInstance(TableMetadata.ItemType);
                    TableMetadata.End1Setter.SetClrValue(tableItem, entity);
                    TableMetadata.End2Setter.SetClrValue(tableItem, collectionItem);
                    tableData.Add(tableItem);
                }
                TableMetadata.Setter.SetClrValue(entity, tableData);
            }
        }
    }

    public class ManyToManyTableMetadata
    {
        public Type ItemType { get; set; }

        public IClrPropertyGetter Getter { get; set; }

        public IClrPropertySetter Setter { get; set; }

        public IClrPropertyGetter End1Getter { get; set; }

        public IClrPropertySetter End1Setter { get; set; }

        public IClrPropertyGetter End2Getter { get; set; }

        public IClrPropertySetter End2Setter { get; set; }
    }

    public class ManyToManyCollectionMetadata
    {
        public Type ItemType { get; set; }

        public IClrPropertyGetter Getter { get; set; }

        public IClrPropertySetter Setter { get; set; }
    }
}
