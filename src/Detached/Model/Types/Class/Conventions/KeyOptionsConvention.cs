using System.Linq;

namespace Detached.Model.Conventions
{
    public class KeyOptionsConvention : ITypeOptionsConvention
    {
        public void Apply(MapperModelOptions modelOptions, ClassTypeOptions typeOptions)
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