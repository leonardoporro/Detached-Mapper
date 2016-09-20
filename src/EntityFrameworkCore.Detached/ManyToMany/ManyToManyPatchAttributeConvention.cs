using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace EntityFrameworkCore.Detached.ManyToMany
{
    /// <summary>
    /// Handles [ManyToMany] attribute.
    /// </summary>
    public class ManyToManyPatchAttributeConvention : NavigationAttributeNavigationConvention<ManyToManyAttribute>
    {
        public override InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation navigation, ManyToManyAttribute attribute)
        {
            relationshipBuilder.ModelBuilder
                               .ConfigureManyToMany(navigation.DeclaringEntityType,
                                                    navigation.GetTargetType(),
                                                    navigation.Name,
                                                    attribute.TableName,
                                                    ConfigurationSource.DataAnnotation);
            return relationshipBuilder;
        }
    }
}
