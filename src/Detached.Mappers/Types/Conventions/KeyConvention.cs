using Detached.Mappers.Annotations;
using Detached.Mappers.Types.Class;
using System.Linq;

namespace Detached.Mappers.Types.Conventions
{
    public class KeyConvention : ITypeConvention
    {
        public void Apply(MapperOptions modelOptions, ClassType typeOptions)
        {
            if (!typeOptions.Members.Any(m => m.IsKey()))
            {
                foreach (ClassTypeMember memberOptions in typeOptions.Members)
                {
                    if (memberOptions.Name == "Id")
                    {
                        memberOptions.IsKey(true);
                        return;
                    }
                }
            }
        }
    }
}