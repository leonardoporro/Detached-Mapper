using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.ManyToMany
{
    public static class ManyToManyModelExtensions
    {
        // a key to save a custom annotation holding the many to many properties.
        const string MANY_TO_MANY_ANNOTATION_KEY = "ManyToManyPatch_PropertyList";

        /// <summary>
        /// Configures the property as a many to many collection.
        /// </summary>
        /// <typeparam name="TEnd1">Type of the related entity.</typeparam>
        /// <typeparam name="TEnd2">Type of the related entity.</typeparam>
        /// <param name="entityTypeBuilder">Entity builder instance.</param>
        /// <param name="selector">Member expression for the configured property.</param>
        /// <param name="tableName">Name of the intermediate table.</param>
        public static void HasManyToMany<TEnd1, TEnd2>(this EntityTypeBuilder<TEnd1> entityTypeBuilder, Expression<Func<TEnd1, ICollection<TEnd2>>> selector, string tableName = null)
            where TEnd1 : class
            where TEnd2 : class
        {
            InternalModelBuilder modelBuilder = new InternalModelBuilder((Model)entityTypeBuilder.Metadata.Model);

            EntityType end1Type = modelBuilder.Metadata.FindEntityType(typeof(TEnd1));
            EntityType end2Type = modelBuilder.Metadata.FindEntityType(typeof(TEnd2));

            string propName = ((MemberExpression)selector.Body).Member.Name;

            ConfigureManyToMany(modelBuilder, end1Type, end2Type, propName, tableName, ConfigurationSource.Explicit);
        }

        /// <summary>
        /// Gets the many to many properties for the given entity.
        /// </summary>
        /// <param name="entityType">Metadata of the entity whose many to many properties are requested.</param>
        /// <returns>An enumeration of ManyToManyNavigation containing info about the property.</returns>
        public static IEnumerable<ManyToManyNavigation> GetManyToManyNavigations(this IEntityType entityType)
        {
            IAnnotation annotation = entityType.FindAnnotation(MANY_TO_MANY_ANNOTATION_KEY);
            if (annotation != null)
                return (IEnumerable<ManyToManyNavigation>)annotation.Value;
            else
                return new ManyToManyNavigation[0];
        }

        /// <summary>
        /// Configures the given property as ManyToMany and creates the intermediate table.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="end1EntityType">An entity type to relate.</param>
        /// <param name="end2EntityType">An entity type to relate.</param>
        /// <param name="propertyName">The name of the many to many property.</param>
        /// <param name="tableName">The name of the intermediate table.</param>
        /// <param name="configSource">The configuration source.</param>
        public static void ConfigureManyToMany(this InternalModelBuilder modelBuilder, EntityType end1EntityType, EntityType end2EntityType, string propertyName, string tableName, ConfigurationSource configSource)
        {
            Type m2mType = typeof(ManyToManyEntity<,>).MakeGenericType(end1EntityType.ClrType, end2EntityType.ClrType);

            InternalEntityTypeBuilder m2mEntityBuilder = modelBuilder.Entity(m2mType, configSource);
            IMutableEntityType m2mEntityType = m2mEntityBuilder.Metadata;

            if (string.IsNullOrEmpty(tableName))
                tableName = end1EntityType.ClrType.Name + end2EntityType.ClrType.Name;

            m2mEntityType.Relational().TableName = tableName;

            IEnumerable<IMutableNavigation> m2mNavigations = m2mEntityType.GetNavigations();
            IMutableNavigation navToEnd1 = m2mNavigations.Single(n => n.Name == "End1");
            IMutableNavigation navToEnd2 = m2mNavigations.Single(n => n.Name == "End2");

            FixForeignKeyPropertyNames(m2mEntityBuilder, navToEnd1, configSource);
            FixForeignKeyPropertyNames(m2mEntityBuilder, navToEnd2, configSource);

            CreatePrimaryKey(m2mEntityBuilder);

            Navigation originalNavigation = RemoveOriginalOneToManyAssociation(end1EntityType, end2EntityType, propertyName);

            ManyToManyNavigation m2mNavigation = new ManyToManyNavigation
            {
                End1 = new ManyToManyNavigationEnd
                {
                    PropertyName = propertyName,
                    EntityType = end1EntityType,
                    Getter = originalNavigation.Getter,
                    Setter = originalNavigation.Setter
                },
                End2 = new ManyToManyNavigationEnd
                {
                    PropertyName = "",
                    EntityType = end2EntityType
                },
                IntermediateEntityType = m2mEntityBuilder.ModelBuilder.Metadata.FindEntityType(m2mType)
            };

            AddManyToManyInfoAnnotation(end1EntityType, m2mNavigation, configSource);
        }

        /// <summary>
        /// Renames [End1][Key] and [End2][Key] foreign key properties to [EntityName][Key].
        /// e.g.: End1Id to UserId and End2Id to RoleId.
        /// </summary>
        /// <param name="m2mEntityBuilder">The entity builder.</param>
        /// <param name="m2mNavigation">The intermediate entity's navigation property being processed.</param>
        /// <param name="configSource">The configuration source.</param>
        static void FixForeignKeyPropertyNames(InternalEntityTypeBuilder m2mEntityBuilder, IMutableNavigation m2mNavigation, ConfigurationSource configSource)
        {
            for (int i = 0; i < m2mNavigation.ForeignKey.Properties.Count; i++)
            {
                IMutableProperty fkProp = m2mNavigation.ForeignKey.Properties[i];
                IMutableProperty keyProp = m2mNavigation.ForeignKey.PrincipalKey.Properties[i];
                InternalPropertyBuilder propBuilder = m2mEntityBuilder.Property(fkProp.Name, fkProp.ClrType, configSource);
                propBuilder.Relational(configSource).ColumnName = keyProp.DeclaringEntityType.ClrType.Name + keyProp.Name;
            }
        }

        /// <summary>
        /// Marks all the intermediate entity properties as Key.
        /// </summary>
        /// <param name="m2mEntityBuilder">The entity builder.</param>
        static void CreatePrimaryKey(InternalEntityTypeBuilder m2mEntityBuilder)
        {
            List<string> pKey = m2mEntityBuilder.Metadata.GetProperties().Select(p => p.Name).ToList();
            m2mEntityBuilder.PrimaryKey(pKey, ConfigurationSource.Explicit);
        }

        /// <summary>
        /// Removes the 1 to * association between End1 and End2.
        /// </summary>
        /// <param name="end1EntityType">Metadata of associated entity.</param>
        /// <param name="end2EntityType">Metadata of associated entity.</param>
        /// <param name="propertyName">The name of the property.</param>
        static Navigation RemoveOriginalOneToManyAssociation(EntityType end1EntityType, EntityType end2EntityType, string propertyName)
        {
            Navigation originalNavigation = end1EntityType.GetNavigations()
                                                      .Where(n => n.Name == propertyName)
                                                      .Single();

            end1EntityType.RemoveNavigation(propertyName);
            end1EntityType.Ignore(propertyName);

            ForeignKey end2FK = originalNavigation.ForeignKey;

            end2EntityType.RemoveForeignKey(end2FK.Properties, end2FK.PrincipalKey, end2FK.PrincipalEntityType);
            foreach (Property prop in end2FK.Properties)
            {
                end2EntityType.RemoveProperty(prop.Name);
            }

            return originalNavigation;
        }

        /// <summary>
        /// Add metadata to be able to recover many to many info from an EntityType.
        /// </summary>
        /// <param name="entityType">The metadata of the entity holding the many to many association.</param>
        /// <param name="item">The metadata of the many to many association.</param>
        /// <param name="configSource">The configuration source.</param>
        static void AddManyToManyInfoAnnotation(EntityType entityType, ManyToManyNavigation item, ConfigurationSource configSource)
        {
            ConventionalAnnotation annotation = entityType.FindAnnotation(MANY_TO_MANY_ANNOTATION_KEY);
            List<ManyToManyNavigation> props;
            if (annotation != null)
            {
                props = (List<ManyToManyNavigation>)annotation.Value;

            }
            else
            {
                props = new List<ManyToManyNavigation>();
                entityType.SetAnnotation(MANY_TO_MANY_ANNOTATION_KEY, props, configSource);
            }
            props.Add(item);
        }
    }
}
