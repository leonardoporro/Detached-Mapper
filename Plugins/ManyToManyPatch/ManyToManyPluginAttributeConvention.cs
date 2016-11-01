using EntityFrameworkCore.Detached.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EntityFrameworkCore.Detached.Plugins.ManyToManyPatch
{
    /// <summary>
    /// Handles [ManyToMany] attribute.
    /// </summary>
    public class ManyToManyPluginAttributeConvention : NavigationAttributeNavigationConvention<ManyToManyAttribute>
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
