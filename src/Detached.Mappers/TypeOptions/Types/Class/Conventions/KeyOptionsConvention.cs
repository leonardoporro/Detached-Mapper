using Detached.Mappers.TypeOptions.Types.Class;
using System.Linq;

namespace Detached.Mappers.TypeOptions.Types.Class.Conventions
{
    public class KeyOptionsConvention : ITypeOptionsConvention
    {
        public void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions)
        {
            if (!typeOptions.Members.Any(m => m.IsKey))
            {
                foreach (ClassMemberOptions memberOptions in typeOptions.Members)
                {
                    if (memberOptions.Name == "Id")
                    {
                        memberOptions.IsKey = true;
                        return;
                    }
                }
            }
        }
    }
}