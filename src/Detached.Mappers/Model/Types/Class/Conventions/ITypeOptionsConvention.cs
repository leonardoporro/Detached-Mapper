namespace Detached.Mappers.Model.Types.Class.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(MapperModelOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
