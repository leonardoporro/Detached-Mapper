namespace Detached.Model.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(MapperModelOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
