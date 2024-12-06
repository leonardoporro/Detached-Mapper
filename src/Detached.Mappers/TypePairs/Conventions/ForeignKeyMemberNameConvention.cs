using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
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
                memberName = GetTargetForeignKeyName(targetMemberName, targetType, mapperOptions);
            }
            else if (sourceType.IsEntity())
            {
                memberName = GetSourceForeignKeyName(targetMemberName, sourceType);
            }

            return memberName;
        }

        string GetSourceForeignKeyName(string targetMemberName, IType sourceType)
        {
            string memberName = null;

            var key = sourceType.GetKeyMember();
            if (key != null)
            {
                if (targetMemberName.EndsWith(key.Name))
                {
                    memberName = targetMemberName.Substring(0, targetMemberName.Length - key.Name.Length);

                }
                else
                {
                    string pluralKeyName = key.Name.Pluralize(false);

                    if (targetMemberName.EndsWith(pluralKeyName))
                    {
                        memberName = targetMemberName.Substring(0, targetMemberName.Length - pluralKeyName.Length);
                    }

                    memberName = memberName.Pluralize(false);
                }

                if (sourceType.GetMember(memberName) == null)
                {
                    memberName = null;
                }
            }

            return memberName;
        }

        string GetTargetForeignKeyName(string targetMemberName, IType targetType, MapperOptions mapperOptions)
        {
            string memberName = null;
            var member = targetType.GetMember(targetMemberName);
            var memberType = mapperOptions.GetType(member.ClrType);


            var key = memberType.IsCollection()
                ? mapperOptions.GetType(memberType.ItemClrType).GetKeyMember()
                : memberType.GetKeyMember();

            if (key != null)
            {
                string entityName;
                string keyName;

                if (memberType.IsCollection())
                {
                    entityName = targetMemberName.Singularize(false);
                    keyName = key.Name.Pluralize(false);
                }
                else
                {
                    entityName = targetMemberName;
                    keyName = key.Name;
                }

                if (keyName.StartsWith(entityName))
                    memberName = keyName;
                else
                    memberName = entityName + keyName;

                if (targetType.GetMember(memberName) != null)
                {
                    memberName = null; // do not map fk to entity if there is already an entity to entity option.
                }
            }

            return memberName;
        }
    }
}
