namespace Detached.Mappers.TypeOptions.Types.Class.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
