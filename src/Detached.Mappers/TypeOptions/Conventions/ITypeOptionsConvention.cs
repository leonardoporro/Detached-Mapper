using Detached.Mappers.TypeOptions.Class;

namespace Detached.Mappers.TypeOptions.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
