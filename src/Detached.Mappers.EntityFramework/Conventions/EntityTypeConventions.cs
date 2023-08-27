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

        public void Apply(MapperOptions mapperOptions, ClassType typeOptions)
        {
            IEntityType entityType = _model.FindEntityType(typeOptions.ClrType);

            if (entityType != null)
            {
                typeOptions.Entity(true);
            }

            if (typeOptions.IsEntity())
            {
                IKey pk = entityType.FindPrimaryKey();

                // don't think we need a hashset here, for 1 or 2 values...
                string[] keyMembers = pk.Properties.Select(p => p.Name).ToArray();

                if (typeOptions.MemberNames != null && typeOptions.MemberNames.Any())
                {
                    foreach (string memberName in typeOptions.MemberNames)
                    {
                        ClassTypeMember member = typeOptions.GetMember(memberName) as ClassTypeMember;
                        member.Key(keyMembers.Contains(memberName));
                    }
                }

                IProperty discriminator = entityType.FindDiscriminatorProperty();
                if (discriminator != null && (entityType.BaseType == null || entityType.IsAbstract()))
                {
                    typeOptions.SetDiscriminatorName(discriminator.Name);

                    foreach (var inheritedType in entityType.Model.GetEntityTypes())
                    {
                        if (IsBaseType(inheritedType, entityType)) //inheritedType.BaseType == entityType)
                        {
                            typeOptions.GetDiscriminatorValues()[inheritedType.GetDiscriminatorValue()] = inheritedType.ClrType;
                        }
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