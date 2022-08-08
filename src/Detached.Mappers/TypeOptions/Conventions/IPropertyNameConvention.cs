namespace Detached.Mappers.TypeOptions.Conventions
{
    public interface IPropertyNameConvention
    {
        string GetSourcePropertyName(ITypeOptions sourceType, ITypeOptions targetType, string memberName);
    }
}
