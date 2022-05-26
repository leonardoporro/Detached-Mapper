using Detached.Mappers.TypeOptions.Class;

namespace Detached.Mappers.TypeOptions.Class.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
