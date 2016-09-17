using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Detached.Conventions
{
    /// <summary>
    /// Handles [Owned] attribute.
    /// </summary>
    public class ManyToManyNavigationAttributeConvention : NavigationAttributeNavigationConvention<ManyToManyAttribute>
    {
        public override InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation navigation, ManyToManyAttribute attribute)
        {
            InternalModelBuilder modelBuilder = relationshipBuilder.ModelBuilder;

            EntityType parentEntityType = navigation.DeclaringEntityType;
            EntityType childEntityType = navigation.GetTargetType();

            Type parentType = parentEntityType.ClrType;
            Type childType = childEntityType.ClrType;

            Type m2mType = typeof(ManyToManyRow<,>).MakeGenericType(parentType, childType);

            InternalEntityTypeBuilder m2mBuilder = modelBuilder.Entity(m2mType, ConfigurationSource.Explicit);

            EntityType m2mEntityType = modelBuilder.Metadata.FindEntityType(m2mType);

            string m2mName = parentType.Name + childType.Name;

            m2mEntityType.Relational().TableName = m2mName;

            List<string> propNames = new List<string>();
            foreach (Navigation nav in m2mEntityType.GetNavigations())
            {
                for (int i = 0; i < nav.ForeignKey.Properties.Count; i++)
                {
                    Property fkProp = nav.ForeignKey.Properties[i];
                    Property keyProp = nav.ForeignKey.PrincipalKey.Properties[i];

                    var internalBuilder = m2mBuilder.Property(fkProp.Name, ConfigurationSource.Explicit);

                    string columnName = keyProp.DeclaringEntityType.ClrType.Name + "_" + keyProp.Name;

                    internalBuilder.Relational(ConfigurationSource.Explicit).ColumnName = columnName;

                    var keyBuilder = keyProp.DeclaringEntityType.Builder;
                    var fkBuilder = fkProp.DeclaringEntityType.Builder;

                    propNames.Add(fkProp.Name);
                }
            }
            m2mBuilder.PrimaryKey(propNames, ConfigurationSource.Explicit);

            InternalEntityTypeBuilder parentBuilder = modelBuilder.Entity(parentEntityType.ClrType, ConfigurationSource.Explicit);
            InternalEntityTypeBuilder childBuilder = modelBuilder.Entity(childEntityType.ClrType, ConfigurationSource.Explicit);

            parentBuilder.Ignore(navigation.Name, ConfigurationSource.Explicit);

            return relationshipBuilder;
        }
    }

    public class ManyToManyRow<T1, T2>
    {
        public T1 End1 { get; set; }

        public T2 End2 { get; set; }
    }
}
