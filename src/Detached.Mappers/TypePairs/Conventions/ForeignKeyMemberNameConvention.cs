using Detached.Mappers.Types;
using Humanizer;
using System;
using System.Text.Json;

namespace Detached.Mappers.TypePairs.Conventions
{
    public class ForeignKeyMemberNameConvention : IMemberNameConvention
    {
        public string GetSourceMemberName(string targetMemberName, IType sourceType, IType targetType, MapperOptions mapperOptions)
        {
            string memberName = null;

            if (targetType.IsEntity())
            {
                var key = targetType.GetKeyMember();

                if (key != null)
                {
                    var member = targetType.GetMember(targetMemberName);
                    var memberType = mapperOptions.GetType(member.ClrType);

                    if (memberType.IsCollection())
                    {
                        memberName = targetMemberName.Singularize(false) + key.Name.Pluralize(false);
                    }
                    else
                    {
                        memberName = targetMemberName + key.Name;
                    }

                    if (targetType.GetMember(memberName) != null)
                    {
                        memberName = null; // do not map fk to entity if there is already an entity to entity option.
                    }
                }
            } 
            else if (sourceType.IsEntity())
            {
                var key = sourceType.GetKeyMember();

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
    }
}
