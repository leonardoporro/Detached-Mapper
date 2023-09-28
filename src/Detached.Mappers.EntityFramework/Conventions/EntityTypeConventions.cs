using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Conventions;
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

        public void Apply(MapperOptions mapperOptions, ClassType type)
        {
            IEntityType entityType = _model.FindEntityType(type.ClrType);

            if (entityType != null && !entityType.IsOwned())
            {
                type.Entity(true);

                SetKey(type, entityType);

                SetDiscriminator(type, entityType);

                SetConcurrencyToken(type, entityType);
            }
        }

        void SetConcurrencyToken(ClassType type, IEntityType entityType)
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.IsConcurrencyToken)
                {
                    type.SetConcurrencyTokenName(property.Name);
                }
            }
        }

        void SetDiscriminator(ClassType type, IEntityType entityType)
        {
            IProperty discriminator = entityType.FindDiscriminatorProperty();
            if (discriminator != null && (entityType.BaseType == null || entityType.IsAbstract()))
            {
                type.SetDiscriminatorName(discriminator.Name);

                foreach (var inheritedType in entityType.Model.GetEntityTypes())
                {
                    if (IsBaseType(inheritedType, entityType))
                    {
                        type.GetDiscriminatorValues()[inheritedType.GetDiscriminatorValue()] = inheritedType.ClrType;
                    }
                }
            }
        }

        void SetKey(ClassType type, IEntityType entityType)
        {
            IKey pk = entityType.FindPrimaryKey();
            if (pk != null)
            {
                // don't think we need a hashset here, for 1 or 2 values...
                string[] keyMembers = pk.Properties.Select(p => p.Name).ToArray();

                if (type.MemberNames != null && type.MemberNames.Any())
                {
                    foreach (string memberName in type.MemberNames)
                    {
                        ClassTypeMember member = type.GetMember(memberName) as ClassTypeMember;
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