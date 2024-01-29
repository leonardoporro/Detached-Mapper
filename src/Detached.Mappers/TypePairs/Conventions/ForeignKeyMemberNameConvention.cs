using Detached.Mappers.Types;
using Humanizer;

namespace Detached.Mappers.TypePairs.Conventions
{
    public class ForeignKeyMemberNameConvention : IMemberNameConvention
    {
        public string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            string memberName = null;

            if (targetType.IsEntity())
            {
                var member = targetType.GetMember(targetMemberName);
                var memberType = mapperOptions.GetType(member.ClrType);

                var key = memberType.IsCollection()
                     ? mapperOptions.GetType(memberType.ItemClrType).GetKeyMember()
                     : memberType.GetKeyMember();
 
                if (key != null)
                {
                    string entityPart;
                    string keyPart;

                    if (memberType.IsCollection())
                    {
                        entityPart = targetMemberName.Singularize(false);
                        keyPart = key.Name.Pluralize(false);
                    }
                    else
                    {
                        entityPart = targetMemberName;
                        keyPart = key.Name;
                    }

                    if (keyPart.StartsWith(entityPart))
                        memberName = keyPart;
                    else
                        memberName = entityPart + keyPart;

                    if (targetType.GetMember(memberName) != null)
                    {
                        memberName = null; // do not map fk to entity if there is already an entity to entity option.
                    }
                }
            }

            return memberName;
        }
    }
}
