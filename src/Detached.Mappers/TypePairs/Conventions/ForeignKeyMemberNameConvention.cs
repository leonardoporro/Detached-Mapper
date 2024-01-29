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
                }
            }

            return memberName;
        }
    }
}
