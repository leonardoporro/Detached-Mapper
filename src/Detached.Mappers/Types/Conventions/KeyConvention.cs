using Detached.Mappers.Types.Class;
using System.Linq;

namespace Detached.Mappers.Types.Conventions
{
    public class KeyConvention : ITypeConvention
    {
        public void Apply(MapperOptions mapperOptions, IType type)
        {
            var classType = type as ClassType;

            if (classType != null && !classType.Members.Any(m => m.IsKey()))
            {
                foreach (ClassTypeMember memberOptions in classType.Members)
                {
                    if (memberOptions.Name == "Id")
                    {
                        memberOptions.Key(true);
                        return;
                    }
                }
            }
        }
    }
}