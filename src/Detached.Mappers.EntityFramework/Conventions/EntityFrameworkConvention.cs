using Detached.Mappers.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace Detached.Mappers.EntityFramework.Conventions
{
    public class EntityFrameworkConvention : ITypeOptionsConvention
    {
        readonly IModel _model;

        public EntityFrameworkConvention(IModel model)
        {
            _model = model;
        }

        public void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions)
        {
            IEntityType entityType = _model.FindEntityType(typeOptions.ClrType);

            if (entityType != null && !entityType.IsOwned())
            {
                typeOptions.IsEntity(true);
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
                        ClassMemberOptions member = typeOptions.GetMember(memberName) as ClassMemberOptions;
                        member.IsKey(keyMembers.Contains(memberName));
                    }
                }

                IProperty discriminator = entityType.FindDiscriminatorProperty();
                if (discriminator != null && entityType.BaseType == null)
                {
                    typeOptions.DiscriminatorName = discriminator.Name;

                    foreach (var inheritedType in entityType.Model.GetEntityTypes())
                    {
                        if (inheritedType.BaseType == entityType)
                        {
                            typeOptions.DiscriminatorValues[inheritedType.GetDiscriminatorValue()] = inheritedType.ClrType;
                        }
                    }
                }
            }
        }
    }
}