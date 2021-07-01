using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;
using Detached.Mappers.Model.Types.Class.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
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
            IEntityType entityType = _model.FindEntityType(typeOptions.Type);

            typeOptions.IsEntity = entityType != null && !entityType.IsOwned();

            if (typeOptions.IsEntity)
            {
                IKey pk = entityType.FindPrimaryKey();

                // don't think we need a hashset here, for 1 or 2 values...
                string[] keyMembers = pk.Properties.Select(p => p.Name).ToArray();

                if (typeOptions.MemberNames != null && typeOptions.MemberNames.Any())
                {
                    foreach (string memberName in typeOptions.MemberNames)
                    { 
                        ClassMemberOptions member = typeOptions.GetMember(memberName) as ClassMemberOptions;
                        member.IsKey = keyMembers.Contains(memberName);
                    }
                }
            }
        }
    }
}