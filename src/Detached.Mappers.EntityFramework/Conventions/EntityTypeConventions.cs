using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace Detached.Mappers.EntityFramework.Conventions
{
    public class EntityTypeConventions : ITypeConvention
    {
        readonly IModel _model;

        public EntityTypeConventions(IModel model)
        {
            _model = model;
        }

        public void Apply(MapperOptions mapperOptions, IType type)
        {
            var classType = type as ClassType;

            if (classType != null)
            {
                IEntityType entityType = _model.FindEntityType(type.ClrType);

                if (entityType != null && !entityType.IsOwned())
                {
                    if (!classType.IsEntityConfigured())
                    {
                        classType.Entity(true);
                    }

                    if (!classType.IsKeyConfigured())
                    {
                        SetKey(classType, entityType);
                    }

                    if (!classType.IsDiscriminatorNameConfigured())
                    {
                        SetDiscriminator(classType, entityType);
                    }

                    if (!classType.IsConcurrencyTokenNameSet())
                    {
                        SetConcurrencyToken(classType, entityType);
                    }
                }
            }
        }

        void SetConcurrencyToken(ClassType classType, IEntityType entityType)
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.IsConcurrencyToken)
                {
                    classType.SetConcurrencyTokenName(property.Name);
                }
            }
        }

        void SetDiscriminator(ClassType classType, IEntityType entityType)
        {
            IProperty discriminator = entityType.FindDiscriminatorProperty();
            if (discriminator != null && (entityType.BaseType == null || entityType.IsAbstract()))
            {
                classType.SetDiscriminatorName(discriminator.Name);

                foreach (var inheritedType in entityType.Model.GetEntityTypes())
                {
                    if (IsBaseType(inheritedType, entityType))
                    {
                        classType.GetDiscriminatorValues()[inheritedType.GetDiscriminatorValue()] = inheritedType.ClrType;
                    }
                }
            }
        }

        void SetKey(ClassType classType, IEntityType entityType)
        {
            IKey pk = entityType.FindPrimaryKey();
            if (pk != null)
            {
                // don't think we need a hashset here, for 1 or 2 values...
                string[] keyMembers = pk.Properties.Select(p => p.Name).ToArray();

                if (classType.MemberNames != null && classType.MemberNames.Any())
                {
                    foreach (string memberName in classType.MemberNames)
                    {
                        ClassTypeMember member = classType.GetMember(memberName) as ClassTypeMember;
                        member.Key(keyMembers.Contains(memberName));
                    }
                }
            }
        }

        bool IsBaseType(IEntityType entityType, IEntityType baseType)
        {
            if (entityType.BaseType == baseType)
            {
                return true;
            }
            else
            {
                if (entityType.BaseType != null)
                {
                    return IsBaseType(entityType.BaseType, baseType);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}