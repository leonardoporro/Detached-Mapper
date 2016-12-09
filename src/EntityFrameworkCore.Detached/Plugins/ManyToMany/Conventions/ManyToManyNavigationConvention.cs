using EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Plugins.ManyToMany.Conventions
{
    public class ManyToManyNavigationConvention : NavigationAttributeNavigationConvention<ManyToManyAttribute>
    {
        public override InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation tableNavigation, ManyToManyAttribute attribute)
        {
            // get entity types for many to many ends.
            EntityType parentEntityType = relationshipBuilder.Metadata.PrincipalEntityType;

            ManyToManyMetadata metadata = new ManyToManyMetadata();
            metadata.CollectionMetadata = BuildCollectionMetadata(parentEntityType, attribute.NavigationPropertyName);
            metadata.TableMetadata = BuildTableMetadata(relationshipBuilder, tableNavigation, metadata.CollectionMetadata.ItemType);

            // add metadata to the property.
            GetOrCreateMetadataList(parentEntityType).Add(metadata);

            return relationshipBuilder;
        }

        static ManyToManyCollectionMetadata BuildCollectionMetadata(EntityType parentEntityType, string name)
        {
            PropertyInfo propInfo = parentEntityType.ClrType.GetRuntimeProperty(name);
            if (propInfo == null)
                throw new Exception($"Navigation property '{name}' couldn't be found.");

            Type[] genericArgs;
            if (!propInfo.PropertyType.GetTypeInfo().IsGenericType ||
                 (genericArgs = propInfo.PropertyType.GetTypeInfo().GenericTypeArguments).Length != 1)
            {
                throw new Exception($"Type '{propInfo.PropertyType.FullName}' is not a valid collection type.");
            }

            Type itemType = genericArgs[0];
            EntityType collectionEntityType = parentEntityType.Model.FindEntityType(itemType);

            ClrPropertyGetterFactory getterFactory = new ClrPropertyGetterFactory();
            ClrPropertySetterFactory setterFactory = new ClrPropertySetterFactory();
            return new ManyToManyCollectionMetadata
            {
                Getter = getterFactory.Create(propInfo),
                Setter = setterFactory.Create(propInfo),
                ItemType = itemType
            };
        }

        static ManyToManyTableMetadata BuildTableMetadata(InternalRelationshipBuilder relationshipBuilder, Navigation tableNavigation, Type collectionType)
        {
            EntityType parentEntityType = relationshipBuilder.Metadata.PrincipalEntityType;

            // get the clr type of the intermediate entity.
            EntityType tableEntityType = tableNavigation.GetTargetType();
            InternalEntityTypeBuilder tableBuilder = relationshipBuilder.ModelBuilder.Entity(tableEntityType.ClrType, ConfigurationSource.DataAnnotation, true);

            // metadata for the intermediate table.
            IEnumerable<Navigation> tableNavigations = tableEntityType.GetNavigations();
            Navigation end1Navigation = tableNavigations.Single(p => p.ClrType == parentEntityType.ClrType);
            Navigation end2Navigation = tableNavigations.Single(p => p.ClrType == collectionType);

            tableNavigation.Owned(ConfigurationSource.DataAnnotation);
            end1Navigation.Associated(ConfigurationSource.DataAnnotation);
            end2Navigation.Associated(ConfigurationSource.DataAnnotation);

            // if there is no FK, build one.
            if (tableEntityType.FindPrimaryKey() == null)
            {
                List<string> pkProps = tableEntityType.GetForeignKeys()
                        .Where(fk => fk.PrincipalEntityType == parentEntityType || fk.PrincipalEntityType.ClrType == collectionType)
                        .OrderBy(fk => fk.PrincipalEntityType != parentEntityType)
                        .SelectMany(fk => fk.Properties)
                        .Select(p => p.Name)
                        .ToList();

                tableBuilder.PrimaryKey(pkProps, ConfigurationSource.Explicit);
            }

            return new ManyToManyTableMetadata
            {
                Getter = tableNavigation.Getter,
                Setter = tableNavigation.Setter,
                End1Getter = end1Navigation.GetGetter(),
                End1Setter = end1Navigation.GetSetter(),
                End2Getter = end2Navigation.GetGetter(),
                End2Setter = end2Navigation.GetSetter(),
                ItemType = tableEntityType.ClrType
            };
        }

        static List<ManyToManyMetadata> GetOrCreateMetadataList(EntityType entityType)
        {
            string annotationKey = typeof(ManyToManyMetadata).FullName;
            var annotation = entityType.FindAnnotation(annotationKey);
            if (annotation != null)
            {
                return annotation.Value as List<ManyToManyMetadata>;
            }
            else
            {
                List<ManyToManyMetadata> list = new List<ManyToMany.ManyToManyMetadata>();
                entityType.SetAnnotation(annotationKey, list, ConfigurationSource.DataAnnotation);
                return list;
            }
        }
    }
}
