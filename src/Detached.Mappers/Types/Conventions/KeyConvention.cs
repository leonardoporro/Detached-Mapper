using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types.Class;

namespace Detached.Mappers.Types.Conventions
{
    public class KeyConvention : ITypeConvention
    {
        public void Apply(MapperOptions mapperOptions, IType type)
        {
            var classType = type as ClassType;

            if (classType != null && classType.IsComplexOrEntity() && !classType.IsKeyDefined())
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