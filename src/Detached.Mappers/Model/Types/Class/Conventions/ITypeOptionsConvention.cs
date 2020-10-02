namespace Detached.Mappers.Model.Types.Class.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
