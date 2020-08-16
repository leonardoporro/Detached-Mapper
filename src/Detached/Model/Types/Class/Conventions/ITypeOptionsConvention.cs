namespace Detached.Model.Conventions
{
    public interface ITypeOptionsConvention
    {
        void Apply(ModelOptions modelOptions, ClassTypeOptions typeOptions);
    }
}
