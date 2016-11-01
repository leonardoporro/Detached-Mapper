using EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkCore.Detached.Plugins.ManyToMany.Conventions
{
    public class ManyToManyNavigationConvention : NavigationAttributeNavigationConvention<ManyToManyAttribute>
    {
        public override InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation navigation, ManyToManyAttribute attribute)
        {
            // get entity types for many to many ends.
            EntityType aEntityType = relationshipBuilder.Metadata.PrincipalEntityType;
            EntityType bEntityType = relationshipBuilder.Metadata.DeclaringEntityType;

            // get the navigation that points to the many to many intermediate entity.
            Navigation m2mNavigation = aEntityType.FindNavigation(attribute.NavigationPropertyName);
            if (m2mNavigation == null)
                throw new Exception($"Navigation property '{attribute.NavigationPropertyName}' couldn't be found");

            // get the clr type of the intermediate entity.
            EntityType m2mEntityType = m2mNavigation.GetTargetType();
            InternalEntityTypeBuilder m2mBuilder = relationshipBuilder.ModelBuilder.Entity(m2mEntityType.ClrType, ConfigurationSource.DataAnnotation, true);

            // if there is no FK, build one.
            if (m2mEntityType.FindPrimaryKey() == null)
            {
                List<string> pkProps = m2mEntityType.GetForeignKeys()
                        .Where(fk => fk.PrincipalEntityType == aEntityType || fk.PrincipalEntityType == bEntityType)
                        .OrderBy(fk => fk.PrincipalEntityType != aEntityType)
                        .SelectMany(fk => fk.Properties)
                        .Select(p => p.Name)
                        .ToList();

                m2mBuilder.PrimaryKey(pkProps, ConfigurationSource.Explicit);
            }

            // create metadata.

            return relationshipBuilder;
        }
    }
}
